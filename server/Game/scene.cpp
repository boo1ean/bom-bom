#include "scene.h"
#include <QGLWidget>
#include <QGraphicsView>

Scene::Scene( QObject *parent) :
    QObject(parent)
{
    //_world = new b2World(b2Vec2());

    QGLWidget *widget = new QGLWidget;

    QGraphicsView *view = new QGraphicsView;
    view->setViewport(widget);

    widget->showFullScreen();
}
