
#include "UnityAppController.h"


@interface SDKAppController: UnityAppController
{
    
}
+(id)Ins;
+(void)CallUnity:(NSString *)funName param:(NSString *)str;
-(UIViewController*)  getViewController;
@end
