using System;

namespace RatingAdjustment.Services
{
   
    public class RatingAdjustmentService
    {
        const double MAX_STARS = 5.0;  // Likert scale
        const double Z = 1.96; // 95% confidence interval

        double _q;
        double _percent_positive;

      
        void SetPercentPositive(double stars)
        {

            _percent_positive = (stars * 20.0) / 100.0;

        }

      
        void SetQ(double number_of_ratings)
        {
            _q = Z * Math.Sqrt(((_percent_positive * (1.0 - _percent_positive)) + ((Z * Z) / (4.0 * number_of_ratings))) / number_of_ratings);
        }

      
        public double Adjust(double stars, double number_of_ratings)
        {
            if (stars <= MAX_STARS)
            {
                SetPercentPositive(stars);
                SetQ(number_of_ratings);
                double low = ((_percent_positive + ((Z * Z) / (2.0 * number_of_ratings)) - _q) / (1.0 + ((Z * Z) / number_of_ratings)));
                return (low / 20.0) * 100.0;
            }
            return 0.0; 
        }
    }
}