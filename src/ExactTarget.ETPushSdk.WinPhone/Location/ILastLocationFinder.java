package com.exacttarget.etpushsdk.location;

import android.location.Location;
import android.location.LocationListener;

public abstract interface ILastLocationFinder
{
  public abstract Location getLastBestLocation(int paramInt, long paramLong);
  
  public abstract void setChangedLocationListener(LocationListener paramLocationListener);
  
  public abstract void cancel();
}


/* Location:           C:\Users\Moreys\Downloads\MobilePushSDK-Android-3.3.0\MobilePushSDK-Android-3.3.0\libs\etsdk-3.3.0.jar
 * Qualified Name:     com.exacttarget.etpushsdk.location.ILastLocationFinder
 * JD-Core Version:    0.7.0.1
 */