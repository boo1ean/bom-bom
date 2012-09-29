#ifndef CLIENT_H
#define CLIENT_H

#include <QObject>
#include <connection.h>

class Client : public QObject
{
    Q_OBJECT

    Connection *connection;
public:
    explicit Client(Connection* connection, QObject *parent = 0);
    
signals:
    
private slots:
    void executeCommand(Command command, QByteArray data);
    
};

#endif // CLIENT_H
