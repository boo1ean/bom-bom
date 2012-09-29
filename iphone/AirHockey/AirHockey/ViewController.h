//
//  ViewController.h
//  AirHockey
//
//  Created by binary studio on 9/29/12.
//  Copyright (c) 2012 binary studio. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface ViewController : UIViewController <UIAccelerometerDelegate> {
      UIAccelerometer *accelerometer;
}
- (IBAction)doRedButton;

@end
