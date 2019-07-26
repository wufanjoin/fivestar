//
//  ios sdk
//
//#import <StoreKit/StoreKit.h>
//#include <iostream>

#import <UIKit/UIKit.h>
#import "WXApi.h"
#import "UnityAppController.h"


enum ESdkSendType
{
    Init=0,
};

//@interface SDKInterface : NSObject<SKPaymentTransactionObserver>
@interface SDKInterface: UnityAppController<WXApiDelegate>
{
    //- (void)setReferrerName      :(nonnull NSString *)referrerName;
    //NSMutableArray* m_transactionArray;
    //std::string m_orderStr; //存储当前处理订单的orderStr(加密字符串)
}
+(id)Ins;
-(NSString *)test;
@end
