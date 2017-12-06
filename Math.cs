using System;

namespace BulletEngine{
    public static class MathExtensions{
        public static double Map(double value, double min1, double max1, double min2, double max2){
            if(max1-min1 != 0){
                value = (value-min1)/(max1-min1) * (max2-min2) + min2;
                return value;
            }
            throw new Exception("Map min1 and max1 are equal!");
        }

        public static float Map(float value, float min1, float max1, float min2, float max2){
            if(max1-min1 != 0){
                value = (value-min1)/(max1-min1) * (max2-min2) + min2;
                return value;
            }
            throw new Exception("Map min1 and max1 are equal!");
        }
    }
}