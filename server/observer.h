#ifndef OBSERVER_H
#define OBSERVER_H
#include <clientscreenobject.h>

class Observer : public ClientScreenObject
{
    Q_OBJECT
public:
    explicit Observer(Connection *connection, QObject *parent = 0);
    
signals:
    
public slots:
    
};

#endif // OBSERVER_H
