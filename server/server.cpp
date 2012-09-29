#include "server.h"
#include <QtCore>
#include <QtNetwork>

#define DEFAULT_PORT 9595

Server::Server(QObject *parent) :
    QObject(parent)
{
    connect(_server, SIGNAL(newConnection()), this, SLOT(receiveConnection()));

    QNetworkConfigurationManager manager;

    if (manager.capabilities() & QNetworkConfigurationManager::NetworkSessionRequired) {

        _networkSession = new QNetworkSession(manager.defaultConfiguration(), this);
        connect(_networkSession, SIGNAL(opened()), this, SLOT(sessionOpened()));
        _networkSession->open();

    } else {

        sessionOpened();

    }
}

void Server::sessionOpened()
{
    _server = new QTcpServer(this);

    if (!_server->listen(QHostAddress::Any, DEFAULT_PORT)) {
        QString message = tr("Unable to start the server: %1.").arg(_server->errorString());
        throw new std::exception();
    }
}

void Server::receiveConnection()
{
    QTcpSocket* socket = _server->nextPendingConnection();

    emit newConnection(new Connection(socket));
}
