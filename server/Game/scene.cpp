#include "scene.h"
#include <QGLWidget>
#include <QGraphicsView>
#include <QGraphicsScene>

Scene::Scene(QObject *parent) :
    QObject(parent)
{
    _world = new b2World(b2Vec2());

    initGraphicsScene();
}

void Scene::initGraphicsScene()
{
    QGraphicsView *view = new QGraphicsView;

    _scene = new QGraphicsScene(this);
    view->setScene(_scene);

    view->showFullScreen();
}
