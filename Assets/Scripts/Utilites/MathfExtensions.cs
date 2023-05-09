namespace Utilites
{
    public class MathfExtensions
    {
        public static int Sum(int end)
        {
            int result = 0;
            for (int i = 1; i < end; i++) result += i;
            return result;
        }
        
        public static int Sum(int start,int end)
        {
            int result = 0;
            for (int i = start; i < end; i++) result += i;
            return result;
        }
    }
}