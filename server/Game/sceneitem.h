#ifndef SCENEITEM_H
#define SCENEITEM_H

#include <QObject>
#include <Box2D.h>

class SceneItem : public QObject
{
    Q_OBJECT
public:
    SceneItem(QObject *parent = 0);

    void update();
    
signals:
    
public slots:

protected:
    b2Body *_body;
    
};

#endif // SCENEITEM_H
