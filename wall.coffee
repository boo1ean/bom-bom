b2Vec2 = Box2D.Common.Math.b2Vec2
b2AABB = Box2D.Collision.b2AABB
b2BodyDef = Box2D.Dynamics.b2BodyDef
b2Body = Box2D.Dynamics.b2Body
b2FixtureDef = Box2D.Dynamics.b2FixtureDef
b2Fixture = Box2D.Dynamics.b2Fixture
b2World = Box2D.Dynamics.b2World
b2MassData = Box2D.Collision.Shapes.b2MassData
b2PolygonShape = Box2D.Collision.Shapes.b2PolygonShape
b2CircleShape = Box2D.Collision.Shapes.b2CircleShape
b2DebugDraw = Box2D.Dynamics.b2DebugDraw
b2MouseJointDef =  Box2D.Dynamics.Joints.b2MouseJointDef

module.exports = class Wall
	constructor: (world, x, y, w, h) ->
		fix = new b2FixtureDef
		fix.density = 1
		fix.friction = 0.5
		fix.restitution = 0.5		
		fix.shape = new b2PolygonShape
		def.shape.SetAsBox w, h
		def = new b2BodyDef
		def.type = b2Body.b2_staticBody
		def.position.Set x, y
		@body = world.CreateBody(def)
		@body.CreateFixture(fix)
