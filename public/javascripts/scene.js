function init() {
         var   b2Vec2 = Box2D.Common.Math.b2Vec2
            ,  b2AABB = Box2D.Collision.b2AABB
         	,	b2BodyDef = Box2D.Dynamics.b2BodyDef
         	,	b2Body = Box2D.Dynamics.b2Body
         	,	b2FixtureDef = Box2D.Dynamics.b2FixtureDef
         	,	b2Fixture = Box2D.Dynamics.b2Fixture
         	,	b2World = Box2D.Dynamics.b2World
         	,	b2MassData = Box2D.Collision.Shapes.b2MassData
         	,	b2PolygonShape = Box2D.Collision.Shapes.b2PolygonShape
         	,	b2CircleShape = Box2D.Collision.Shapes.b2CircleShape
         	,	b2DebugDraw = Box2D.Dynamics.b2DebugDraw
            ,  b2MouseJointDef =  Box2D.Dynamics.Joints.b2MouseJointDef
            ;
         
         var world = new b2World(
               new b2Vec2(0, 0)    //gravity
            ,  true                 //allow sleep
         );
         
         var fixDef = new b2FixtureDef;
         fixDef.density = 1.0;
         fixDef.friction = 0.5;
         fixDef.restitution = 0.2;
         
         var bodyDef = new b2BodyDef;
         
         //create ground
         bodyDef.type = b2Body.b2_staticBody;
         fixDef.shape = new b2PolygonShape;
         fixDef.shape.SetAsBox(20, 2);
         bodyDef.position.Set(10, 400 / 30 + 1.8);
         world.CreateBody(bodyDef).CreateFixture(fixDef);
         bodyDef.position.Set(10, -1.8);
         world.CreateBody(bodyDef).CreateFixture(fixDef);
         fixDef.shape.SetAsBox(2, 14);
         bodyDef.position.Set(-1.8, 13);
         world.CreateBody(bodyDef).CreateFixture(fixDef);
         bodyDef.position.Set(21.8, 13);
         world.CreateBody(bodyDef).CreateFixture(fixDef);
         
         
         /*//create some objects
         bodyDef.type = b2Body.b2_dynamicBody;
         for(var i = 0; i < 10; ++i) {
            if(Math.random() > 0.5) {
               fixDef.shape = new b2PolygonShape;
               fixDef.shape.SetAsBox(
                     Math.random() + 0.1 //half width
                  ,  Math.random() + 0.1 //half height
               );
            } else {
               fixDef.shape = new b2CircleShape(
                  Math.random() + 0.1 //radius
               );
            }
            bodyDef.position.x = Math.random() * 10;
            bodyDef.position.y = Math.random() * 10;
            world.CreateBody(bodyDef).CreateFixture(fixDef);
         }*/

         bodyDef.type = b2Body.b2_dynamicBody;
         fixDef.shape = new b2CircleShape(1);
         bodyDef.position.x = 0;
         bodyDef.position.y = 0;
         fixDef.restitution = 1;
         fixDef.friction = 0;         
         var ball = world.CreateBody(bodyDef);
         ball.CreateFixture(fixDef);

         ball.ApplyImpulse(new b2Vec2(100, 100), new b2Vec2(0, 0));

         fixDef.shape = new b2PolygonShape;
         fixDef.shape.SetAsBox(2, 0.5);
         bodyDef.position.x = 2;
         bodyDef.position.y = 0;
         fixDef.restitution = 0.5;
         fixDef.friction = 0.5;
         var plank = world.CreateBody(bodyDef);
         plank.CreateFixture(fixDef);
         
         //setup debug draw
         var debugDraw = new b2DebugDraw();
			debugDraw.SetSprite(document.getElementById("canvas").getContext("2d"));
			debugDraw.SetDrawScale(30.0);
			debugDraw.SetFillAlpha(0.5);
			debugDraw.SetLineThickness(1.0);
			debugDraw.SetFlags(b2DebugDraw.e_shapeBit | b2DebugDraw.e_jointBit);
			world.SetDebugDraw(debugDraw);
         
         window.setInterval(update, 1000 / 60);
         
         //mouse
         
         var mouseX, mouseY, mousePVec, isMouseDown, selectedBody, mouseJoint;
         var canvasPosition = getElementPosition(document.getElementById("canvas"));
         
         document.addEventListener("mousedown", function(e) {
            isMouseDown = true;
            handleMouseMove(e);
            document.addEventListener("mousemove", handleMouseMove, true);
         }, true);
         
         document.addEventListener("mouseup", function() {
            document.removeEventListener("mousemove", handleMouseMove, true);
            isMouseDown = false;
            mouseX = undefined;
            mouseY = undefined;
         }, true);
         
         function handleMouseMove(e) {
            mouseX = (e.clientX - canvasPosition.x) / 30;
            mouseY = (e.clientY - canvasPosition.y) / 30;
         };
         
         function getBodyAtMouse() {
            mousePVec = new b2Vec2(mouseX, mouseY);
            var aabb = new b2AABB();
            aabb.lowerBound.Set(mouseX - 0.001, mouseY - 0.001);
            aabb.upperBound.Set(mouseX + 0.001, mouseY + 0.001);
            
            // Query the world for overlapping shapes.

            selectedBody = null;
            world.QueryAABB(getBodyCB, aabb);
            return selectedBody;
         }

         function getBodyCB(fixture) {
            if(fixture.GetBody().GetType() != b2Body.b2_staticBody) {
               if(fixture.GetShape().TestPoint(fixture.GetBody().GetTransform(), mousePVec)) {
                  selectedBody = fixture.GetBody();
                  return false;
               }
            }
            return true;
         }
         
         //update
         
         function update() {
         
            if(isMouseDown && (!mouseJoint)) {
               var body = getBodyAtMouse();
               if(body) {
                  var md = new b2MouseJointDef();
                  md.bodyA = world.GetGroundBody();
                  md.bodyB = body;
                  md.target.Set(mouseX, mouseY);
                  md.collideConnected = true;
                  md.maxForce = 300.0 * body.GetMass();
                  mouseJoint = world.CreateJoint(md);
                  body.SetAwake(true);
               }
            }
            
            if(mouseJoint) {
               if(isMouseDown) {
                  mouseJoint.SetTarget(new b2Vec2(mouseX, mouseY));
               } else {
                  world.DestroyJoint(mouseJoint);
                  mouseJoint = null;
               }
            }
         
            world.Step(1 / 60, 12, 10);
            world.DrawDebugData();
            world.ClearForces();
         };
         
         //helpers
         
         //http://js-tut.aardon.de/js-tut/tutorial/position.html
         function getElementPosition(element) {
            var elem=element, tagname="", x=0, y=0;
           
            while((typeof(elem) == "object") && (typeof(elem.tagName) != "undefined")) {
               y += elem.offsetTop;
               x += elem.offsetLeft;
               tagname = elem.tagName.toUpperCase();

               if(tagname == "BODY")
                  elem=0;

               if(typeof(elem) == "object") {
                  if(typeof(elem.offsetParent) == "object")
                     elem = elem.offsetParent;
               }
            }

            return {x: x, y: y};
         }

         $("#coo").text("test");

         var socket = io.connect('http://192.168.1.133:3000');
         socket.on('acceleration', function (data) {
            var c = 3;
            data[0] = Math.round(data[0]*c);
            data[1] = Math.round(data[1]*c);

            $("#coo").text(data[0].toFixed(2) + " " + data[1].toFixed(2) + " " + data[2].toFixed(2));
            
           plank.ApplyImpulse(new b2Vec2(data[0], data[1]), plank.GetWorldCenter());
         });

      };
