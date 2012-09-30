#include "scene.h"
#include <QGLWidget>
#include <QGraphicsView>
#include <QGraphicsScene>
#include <QGraphicsItem>
#include <QDebug>

Scene::Scene(float w, float h, QObject *parent) :
    QObject(parent)
{
    _time = new QTimer;

    connect(_time, SIGNAL(timeout()), this, SLOT(onNewFrame()));

    initGraphicsScene();

    _world = new b2World(b2Vec2(0, -9.8f));

    float left = -w/2;
    float right = w/2;
    float top = h/2;
    float bottom = -h/2;

    // top wall
    _walls.push_back(new Wall(this, b2Vec2(left, top), b2Vec2(right, top)));
    // left wall
    _walls.push_back(new Wall(this, b2Vec2(left, top), b2Vec2(left, bottom)));
    // right wall
    _walls.push_back(new Wall(this, b2Vec2(right, top), b2Vec2(right, bottom)));
    // bottom wall
    _walls.push_back(new Wall(this, b2Vec2(left, bottom), b2Vec2(right, bottom)));

    _ball = new Ball(this, h/20);

    _time->start(1000/60);
}

void Scene::initGraphicsScene()
{
    QGraphicsView *view = new QGraphicsView;

    _scene = new QGraphicsScene(this);
    view->setScene(_scene);

    view->showFullScreen();
}

void Scene::onNewFrame()
{
    _world->Step(1.0f/60.0f, 100, 100);
    _ball->update();
}

b2World* Scene::getPhysics() const
{
    return _world;
}

QGraphicsScene* Scene::getGraphics() const
{
    return _scene;
}
