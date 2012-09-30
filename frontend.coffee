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

class Wall
   constructor: (world, x, y, w, h) ->
      fix = new b2FixtureDef
      fix.density = 1
      fix.friction = 0.5
      fix.restitution = 0.5      
      fix.shape = new b2PolygonShape      
      fix.shape.SetAsBox w, h
      def = new b2BodyDef
      def.type = b2Body.b2_staticBody
      def.position.Set x, y
      @body = world.CreateBody(def)
      @body.CreateFixture(fix)

class Ball
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

class Plank
    constructor: (world, x, y, h, w)->
         b2FixtureDef.shape.SetAsBox w, h
         b2FixtureDef.shape       = new b2PolygonShape
         b2BodyDef.position.x     = x
         b2BodyDef.position.y     = y
         b2FixtureDef.restitution = 0.5
         b2FixtureDef.friction    = 0.5
         @plank = world.CreateBody b2BodyDef
         @plank.CreateFixture b2FixtureDef

      accelerate: (acceleration) ->
         @plank.ApplyImpulse new b2Vec2(acceleration.x, acceleration.y), @plank.GetWorldCenter()

class Scene
   constructor: (options) ->
      @options = options
      gravity = new b2Vec2 0, 0
      @world = new b2World gravity, true
      wall_width = 0.1
      new Wall @world, 0, 0, options.width, wall_width
      new Wall @world, 0, 0, wall_width, options.height
      new Wall @world, options.width, 0, wall_width, options.height
      new Wall @world, 0, options.height, options.height, wall_width
      @ball = new Ball @world, options.ball_radius
      @planks = {}

   add: (name) ->
      @planks[name] = new Plank @world, options.width/2, options.height/2, options.height/3, options.width/20

   accelerate: (name, acceleration) ->
      @planks[name].accelerate acceleration

   update: ->
      @world.Step 1/60, 12, 10

class Render
   constructor: (options) ->
      canvas = document.getElementById(options.canvas_id)
      context = canvas.getContext("2d")
      draw = new b2DebugDraw
      draw.SetSprite context
      draw.SetDrawScale 30.0
      draw.SetFillAlpha 0.5
      draw.SetLineThickness 1.0
      draw.SetFlags (b2DebugDraw.e_shapeBit | b2DebugDraw.e_jointBit)
      options.world.SetDebugDraw draw

class Game
   constructor: (server) ->
      @scene = new Scene
         width: 800
         height: 600
      @render = new Render
         canvas_id: "canvas"
         world: @scene.world         
      @socket = io.connect server
      @socket.on "client", (name) =>
         @scene.add name
      @socket.on "acceleration", (data) =>
         @scene.accelerate data.name, data.acceleration
      setInterval @scene.update, 1000/60

window.onload = ->
   new Game "http://192.168.1.133:3000"