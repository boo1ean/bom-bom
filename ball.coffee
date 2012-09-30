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

module.exports = class Ball
    constructor: (world, radius)->
         b2BodyDef.type           = b2Body.b2_dynamicBody
         b2BodyDef.position.x     = 0
         b2BodyDef.position.y     = 0
         b2FixtureDef.shape       = new b2CircleShape radius
         b2FixtureDef.restitution = 1
         b2FixtureDef.friction    = 0
         ball = world.CreateBody bodyDef
         ball.CreateFixture b2FixtureDef
         ball.ApplyImpulse new b2Vec2(100, 100), new b2Vec2(0, 0)
