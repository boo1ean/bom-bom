#include "wall.h"
#include "scene.h"

Wall::Wall(Scene *scene, const b2Vec2& start, const b2Vec2& end, QObject *parent) :
    QObject(parent)
{
    b2BodyDef def;
    def.position.Set((start.x + end.x)/2, (start.y + end.y)/2);
    _body = scene->getPhysics()->CreateBody(&def);

    b2EdgeShape shape;
    shape.Set(start, end);
    _body->CreateFixture(&shape);

    _body->SetUserData(scene->getGraphics()->addLine(start.x, start.y, end.x, end.y));
}
