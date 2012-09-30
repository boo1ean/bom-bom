#include "ball.h"
#include "scene.h"
#include <QGraphicsItem>
#include <QDebug>

Ball::Ball(Scene *scene, float r, QObject *parent) :
    QObject(parent)
{
    b2BodyDef def;
    def.type = b2_dynamicBody;
    def.position.SetZero();

    _body = scene->getPhysics()->CreateBody(&def);

    b2CircleShape shape;
    shape.m_radius = r;
    b2FixtureDef fixtureDef;
    fixtureDef.shape = &shape;
    fixtureDef.restitution = 1;
    fixtureDef.friction = 0;
    _body->CreateFixture(&fixtureDef);

    _body->SetUserData(scene->getGraphics()->addEllipse(-r, -r, r*2, r*2));

    _body->ApplyLinearImpulse(b2Vec2(500, 500), b2Vec2());
}

void Ball::update()
{
    QGraphicsItem *item = (QGraphicsItem *)_body->GetUserData();
    const b2Vec2& pos = _body->GetPosition();
    item->setPos(pos.x, pos.y);
}
