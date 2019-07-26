//
//  BMKLocation.h
//  LocationComponent
//
//  Created by baidu on 2017/8/16.
//  Copyright © 2017年 baidu. All rights reserved.
//

#ifndef BMKLocation_h
#define BMKLocation_h

#import <CoreLocation/CoreLocation.h>
#import "BMKLocationReGeocode.h"

/** 
 * BMKLocationProvider 位置数据来源，分iOS系统定位和其他定位服务结果两种，目前仅支持iOS系统定位服务
 *
 */
typedef NS_ENUM(int, BMKLocationProvider) {
    
    BMKLocationProviderIOS = 0,           //!<位置来源于iOS本身定位
    BMKLocationProviderOther          //!<位置来源于其他定位服务
    
};

///描述百度iOS 定位数据
@interface BMKLocation : NSObject

///BMKLocation 位置数据
@property(nonatomic, copy, readonly) CLLocation * _Nullable location;

///BMKLocation 地址数据
@property(nonatomic, copy) BMKLocationReGeocode * _Nullable rgcData;

///BMKLocation 位置来源
@property(nonatomic, assign) BMKLocationProvider provider;

///BMKLocation 位置ID
@property(nonatomic, retain) NSString * _Nullable locationID;

/*
 *  floorString
 *
 *  Discussion:
 *    室内定位成功时返回的楼层信息，ex:f1
 */
@property(readonly, nonatomic, copy, nullable) NSString *floorString;

/*
 *  buildingID
 *
 *  Discussion:
 *    室内定位成功时返回的百度建筑物ID
 */
@property(readonly, nonatomic, copy, nullable) NSString *buildingID;

/*
 *  buildingName
 *
 *  Discussion:
 *    室内定位成功时返回的百度建筑物名称
 */
@property(readonly, nonatomic, copy, nullable) NSString *buildingName;


/*
 *  extraInfo
 *
 *  Discussion:
 *    定位附加信息，如停车位code识别结果、停车位code示例、vdr推算结果置信度等
 */
@property(readonly, nonatomic, copy, nullable) NSDictionary * extraInfo;

/**
 *  @brief 初始化BMKLocation实例
 *  @param loc CLLocation对象
 *  @param rgc BMKLocationReGeocode对象
 *  @return BMKLocation id
 */
- (id _Nonnull)initWithLocation:(CLLocation * _Nullable)loc withRgcData:(BMKLocationReGeocode * _Nullable)rgc;

/**
 *  @brief 构造BMKLocation
 *  @param location CLLocation
 *  @param floorString 楼层字符串
 *  @param buildingID 建筑物ID
 *  @param buildingName 建筑物名称
 *  @param info 位置附加信息
 *  @return BMKLocation id
 */
-(id _Nonnull)initWithLocation:(CLLocation * _Nullable)location floorString:(NSString * _Nullable)floorString buildingID:(NSString * _Nullable)buildingID
                  buildingName:(NSString * _Nullable)buildingName extraInfo:(NSDictionary * _Nullable)info withRgcData:(BMKLocationReGeocode * _Nullable)rgc;


@end

#endif /* BMKLocation_h */
