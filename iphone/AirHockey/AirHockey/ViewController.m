//
//  ViewController.m
//  AirHockey
//
//  Created by binary studio on 9/29/12.
//  Copyright (c) 2012 binary studio. All rights reserved.
//

#import "ViewController.h"
#import "GCDAsyncSocket.h"
#include <netinet/tcp.h>
#include <netinet/in.h>

@interface ViewController ()

@end

@implementation ViewController

GCDAsyncSocket* asyncSocket;

- (IBAction) doRedButton {
	exit(0);
}

- (void)viewDidLoad
{
    [super viewDidLoad];
	dispatch_queue_t mainQueue = dispatch_get_main_queue();
	
	asyncSocket = [[GCDAsyncSocket alloc] initWithDelegate:self delegateQueue:mainQueue];
    //NSString *host = @"google.com";    uint16_t port = 80;
    NSString *host = @"192.168.1.2";    uint16_t port = 9595;
    
    //DDLogInfo(@"Connecting to \"%@\" on port %hu...", host, port);
    //self.viewController.label.text = @"Connecting...";
    
    NSError *error = nil;
    NSLog(@"connecting");
    if (![asyncSocket connectToHost:host onPort:port error:&error])
    {
        NSLog(@"error");
        //DDLogError(@"Error connecting: %@", error);
        //self.viewController.label.text = @"Oops";
    }

    NSMutableData* data = [NSMutableData dataWithCapacity:8];
    int cmd = CFSwapInt32BigToHost(1), iPhoneId = CFSwapInt32BigToHost(3), nameLength = CFSwapInt32BigToHost(6);
    [data appendBytes:&cmd length:4];
    [data appendBytes:&iPhoneId length:4];
    cmd = CFSwapInt32BigToHost(2);
    [data appendBytes:&cmd length:4];
    [data appendBytes:&nameLength length:4];
    NSString* name = @"iPhone";
    [data appendData:[name dataUsingEncoding:NSUTF16LittleEndianStringEncoding]];
    [asyncSocket writeData:data withTimeout:999 tag:0];
    
    self->accelerometer = [UIAccelerometer sharedAccelerometer];
    self->accelerometer.updateInterval = .01;
    self->accelerometer.delegate = self;
}

- (void)accelerometer:(UIAccelerometer *)accelerometer didAccelerate:(UIAcceleration *)acceleration {
    NSMutableData* data = [NSMutableData dataWithCapacity:8];
    int cmd = 0;
    [data appendBytes:&cmd length:4];
    float f;
    f = acceleration.x;
    [data appendBytes:&f length:4];
    f = acceleration.y;
    [data appendBytes:&f length:4];
    f = acceleration.z;
    [data appendBytes:&f length:4];
    f = 10;
    [data appendBytes:&f length:4];
    [asyncSocket writeData:data withTimeout:999 tag:0];
}

- (void)socket:(GCDAsyncSocket *)sock didConnectToHost:(NSString *)host port:(UInt16)port
{
    [asyncSocket performBlock:^{
        int fd = [asyncSocket socketFD];
        int on = 1;
        if (setsockopt(fd, IPPROTO_TCP, TCP_NODELAY, (char*)&on, sizeof(on)) == -1) {                   }
    }];
    NSLog(@"connected");
}

- (void)socketDidSecure:(GCDAsyncSocket *)sock
{
    NSLog(@"socketDidSecure");
}

- (void)socket:(GCDAsyncSocket *)sock didWriteDataWithTag:(long)tag
{
    NSLog(@"didWriteDataWithTag");
}

- (void)socket:(GCDAsyncSocket *)sock didReadData:(NSData *)data withTag:(long)tag
{
    NSLog(@"didReadData");
}

- (void)socketDidDisconnect:(GCDAsyncSocket *)sock withError:(NSError *)err
{
    exit(1);
    NSLog(@"disconnect");
}

@end
