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


    ///////////////////////////////////////////////


    b2BodyDef def;
    def.type = b2_dynamicBody;

    _ball = _world->CreateBody(&def);

    b2CircleShape shape;
    shape.m_radius = 10;

    b2FixtureDef ballFixture;

    ballFixture.shape = &shape;
    /*ballFixture.density = 1;
    ballFixture.friction = 1;
    ballFixture.restitution = 1;*/

    _ball->CreateFixture(&ballFixture);

    _ball->SetUserData(_scene->addEllipse(0, 0, 10, 10));

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

    const b2Vec2& pos = _ball->GetPosition();

    qDebug() << pos.x << " " << pos.y;

    QGraphicsItem* item = (QGraphicsItem*)_ball->GetUserData();
    item->setPos(pos.x, -pos.y);
}

