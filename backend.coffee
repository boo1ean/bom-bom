express = require "express"
net = require "net"
io = require "socket.io"

express = express.createServer()
io = io.listen express

class Client
	constructor: (socket) ->
		@name = socket.remoteAddress + ":" + socket.remotePort		
		io.sockets.emit "client", @name
		socket.on "data", (data) =>
			try
				io.sockets.emit "acceleration",
					name: @name
					vector: [
						data.readFloatLE(0),
						data.readFloatLE(4),
						data.readFloatLE(8),
					]
			catch exception
				console.log "Can't understand client. #{exception}."

class Server
	constructor: (port) ->
		@clients = []
		tcp_server = net.createServer (socket) =>
			@clients.push new Client socket
		tcp_server.listen (port or 5000)


new Server

express.get "/", (req, res) ->
	res.sendfile "index.html"

express.get "/frontend.js", (req, res) ->
	res.sendfile "frontend.js"

express.listen 3000