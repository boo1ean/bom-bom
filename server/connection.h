#ifndef CONNECTION_H
#define CONNECTION_H

#include <QObject>
#include <QByteArray>
#include <QTcpSocket>
#include <Command.h>

class Connection : public QObject
{
    Q_OBJECT

    QTcpSocket* socket;
public:
    explicit Connection(QTcpSocket* socket, QObject *parent = 0);
    
signals:

    void newCommand(Command command, const QByteArray& data);
    
private slots:
    void onReadyRead();

};

#endif // CONNECTION_H
