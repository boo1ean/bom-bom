#ifndef WALL_H
#define WALL_H

#include <QObject>
#include <Box2D.h>

class Scene;

class Wall : public QObject
{
    Q_OBJECT
public:
    Wall(Scene *scene, const b2Vec2& start, const b2Vec2& end, QObject *parent = 0);
    
signals:
    
private slots:

private:
    b2Body *_body;
};

#endif // WALL_H
