b2Vec2          = Box2D.Common.Math.b2Vec2
b2AABB          = Box2D.Collision.b2AABB
b2BodyDef       = Box2D.Dynamics.b2BodyDef
b2Body          = Box2D.Dynamics.b2Body
b2FixtureDef    = Box2D.Dynamics.b2FixtureDef
b2Fixture       = Box2D.Dynamics.b2Fixture
b2World         = Box2D.Dynamics.b2World
b2MassData      = Box2D.Collision.Shapes.b2MassData
b2PolygonShape  = Box2D.Collision.Shapes.b2PolygonShape
b2CircleShape   = Box2D.Collision.Shapes.b2CircleShape
b2DebugDraw     = Box2D.Dynamics.b2DebugDraw
b2MouseJointDef = Box2D.Dynamics.Joints.b2MouseJointDef

module.exports = class Plank
    constructor: (world, x, y, h, w)->
         b2FixtureDef.shape.SetAsBox w, h
         b2FixtureDef.shape       = new b2PolygonShape
         b2BodyDef.position.x     = x
         b2BodyDef.position.y     = y
         b2FixtureDef.restitution = 0.5
         b2FixtureDef.friction    = 0.5
         plank = world.CreateBody b2BodyDef
         plank.CreateFixture b2FixtureDef
