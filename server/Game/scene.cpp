#include "scene.h"

Scene::Scene(QObject *parent) :
    QObject(parent)
{
    _world = new b2World(b2Vec2());
}
