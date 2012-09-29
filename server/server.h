#ifndef SERVER_H
#define SERVER_H

#include <QtNetwork>

class Connection;

class Server : public QObject
{
    Q_OBJECT

    QNetworkSession* _networkSession;
    QTcpServer* _server;

public:
    explicit Server(QObject *parent = 0);
    
signals:

    void newConnection(Connection*);

private slots:

    void sessionOpened();
    void receiveConnection();
};

#endif // SERVER_H
