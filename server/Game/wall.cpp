#include "wall.h"
#include "scene.h"
#include <QDebug>

Wall::Wall(Scene *scene, const b2Vec2& start, const b2Vec2& end, QObject *parent) :
    QObject(parent)
{
    float x = (start.x + end.x)/2;
    float y = (start.y + end.y)/2;

    b2BodyDef def;
    def.position.Set(x, y);
    _body = scene->getPhysics()->CreateBody(&def);

    b2PolygonShape shape;
    shape.SetAsBox((end.x - start.x)/2, (end.y - start.y)/2);
    b2FixtureDef fixtureDef;
    fixtureDef.shape = &shape;
    fixtureDef.restitution = 1;
    fixtureDef.friction = 0;
    _body->CreateFixture(&fixtureDef);

    _body->SetUserData(scene->getGraphics()->addLine(start.x, start.y, end.x, end.y));
}
