#include "plank.h"
#include "scene.h"
#include <QDebug>

Plank::Plank(Scene *scene, const b2Vec2& c, float r, QObject *parent) :
    SceneItem(parent)
{
    b2BodyDef def;
    def.type = b2_dynamicBody;
    def.position.Set(c.x, c.y);

    _body = scene->getPhysics()->CreateBody(&def);

    b2CircleShape shape;
    shape.m_radius = r;
    b2FixtureDef fixtureDef;
    fixtureDef.shape = &shape;
    fixtureDef.restitution = 1;
    fixtureDef.friction = 0;
    _body->CreateFixture(&fixtureDef);

    _body->SetUserData(scene->getGraphics()->addEllipse(-r, -r, r*2, r*2));
}
