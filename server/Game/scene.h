#ifndef SCENE_H
#define SCENE_H

#include <QObject>
#include <Box2D.h>
#include <QGraphicsScene>
#include <QTimer>
#include "wall.h"
#include "ball.h"

class Scene : public QObject
{
    Q_OBJECT

    b2World *_world;
    QGraphicsScene *_scene;

    QList<Wall*> _walls;
    Ball *_ball;

    QTimer *_time;

    QList<SceneItem *> _updatables;

    void initGraphicsScene(float w, float h);

public:
    Scene(float w, float h, QObject *parent = 0);

    b2World* getPhysics() const;
    QGraphicsScene* getGraphics() const;
    
signals:
    
private slots:
    void onNewFrame();
    
};

#endif // SCENE_H
