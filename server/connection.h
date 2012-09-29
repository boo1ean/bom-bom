#ifndef CONNECTION_H
#define CONNECTION_H

#include <QObject>
#include <QByteArray>
#include <QTcpSocket>

class Connection : public QObject
{
    Q_OBJECT
public:
    explicit Connection(QTcpSocket* socket, QObject *parent = 0);
    
signals:

    void newCommand(Command command, const QByteArray& data);
    
private slots:

    // todo listen socket
    
};

#endif // CONNECTION_H
