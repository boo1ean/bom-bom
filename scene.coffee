module.exports = class Scene
	constructor: (options) ->
		wall_width = 0.1
		new Wall this, 0, 0, options.width, wall_width
		new Wall this, 0, 0, wall_width, options.height
		new Wall this, options.width, 0, wall_width, options.height
		new Wall this, 0, options.height, options.height, wall_width
		@ball = new Ball this, options.ball_radius