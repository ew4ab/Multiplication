using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace Multiplication
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Regex myReg = new Regex(@"\D");
            string firstLine, secondLine;
            
            //Ввод первого числа - firstLine
            do
            {
                Console.WriteLine("Enter first number(should include digits only):");
                firstLine = Console.ReadLine();
            }
            while (myReg.IsMatch(firstLine)==true | firstLine.Length==0);


            //Ввод второго числа - secondLine
            do
            {
                Console.WriteLine("\r\nEnter second number(should include digits only):");
                secondLine = Console.ReadLine();
            }
            while (myReg.IsMatch(secondLine) == true | secondLine.Length==0);

            //Конвертация введенных чисел в double, умножение и вывод результата (для последующего сравнения)
            double firstDouble = Convert.ToDouble(firstLine);
            double secondDouble = Convert.ToDouble(secondLine);
            double totalDouble = firstDouble * secondDouble;
            Console.WriteLine("\r\nThis is result of converting to double and multiplication after that:\r\n" + totalDouble);

            //Конвертация первого длинного числа в массив int - firstArray
            List<int> firstArray = new List<int>();
            for (int i=0; i<=firstLine.Length-1; i++)
            {
                firstArray.Add(Convert.ToInt32(firstLine[i]-'0')); //При переводе char в Int - без нолика никак :)
            }

            //Конвертация второго длинного числа в массив int - secondArray
            List<int> secondArray = new List<int>();
            for (int i=0; i<=secondLine.Length-1; i++)
            {
                secondArray.Add(Convert.ToInt32(secondLine[i] - '0')); //При переводе char в Int - без нолика никак :)
            }

            /*Сложение "в столбик" разбивается на две операции:
             * - умножение массива цифр на цифру (метод MultArrayByDigit) и
             * - сложение двух массивов (метод SumArrays).
             */

            List<int> totalOfMultiplication = new List<int>();
            List<int> result = new List<int>();
            

            int iterationsQty = firstArray.Count()-1;
            
            //Выборка из массива делается с конца к началу
            for (int k=iterationsQty; k>=0; k--)
            {
                //Умножаем второй массив на выборку из первого
                totalOfMultiplication = MultArrayByDigit(secondArray, firstArray[k]);

                //При каждой итерации (кроме первой), согласно правилам умножения "в столбик"
                //добавляем дополнительный ноль в конец. Количество нулей равняется количеству пройденных
                //итераций, минус 1.
                if(k<iterationsQty)
                {
                    for(int s=iterationsQty-k-1; s>=0; s--)
                    {
                        totalOfMultiplication.Add(0);
                    }                    
                }

                //При первой итерации результатом будет являться totalOfMultiplication
                //записываем это значение в result
                if(k==iterationsQty)
                {
                    result = totalOfMultiplication;
                }
                //иначе складываем result и totalOfMultiplication. результат пишем в result
                else
                {
                    result = SumArrays(result, totalOfMultiplication);
                }
            }

            //Вывод результата умножения двух массивов
            Console.WriteLine("\r\nThis is right result of multiplication:");
            foreach (int el in result)
            {
                Console.Write(el);
            }

            Console.WriteLine("\r\n\r\nPress any key to stop the programm...");
            Console.ReadKey();

        }

        //Метод для сложения двух массивов цифр. Возвращает массив totalSum
        public static List<int> SumArrays(List<int> array1, List<int> array2)
        {
            List<int> totalSum = new List<int>();
            int array1Lenght = 0, array2Lenght = 0, maxLenght=0, 
                arrayDifference=0, extra=0, tempInt=0;

            //Массивы удобно складывать если у них одинаковая длина.
            //Если длина массивов разная, начало короткого массива заполняем нулями
            array1Lenght = array1.Count;
            array2Lenght = array2.Count;

            if (array1Lenght != array2Lenght)
            {
                if(array1Lenght>array2Lenght)
                {
                    maxLenght = array1Lenght;
                    arrayDifference = array1Lenght - array2Lenght;
                    for(int i=0; i<=arrayDifference-1; i++)
                    {
                        array2.Insert(0, 0);
                    }
                }
                else
                {
                    maxLenght = array2Lenght;
                    arrayDifference = array2Lenght - array1Lenght;
                    for (int i = 0; i <= arrayDifference-1; i++)
                    {
                        array1.Insert(0, 0);
                    }
                }
            }
            else
            {
                maxLenght = array1Lenght;
            }

            //Собственно, сложение массивов
            for(int i=maxLenght-1; i>=0; i--)//Перевор элементов массива с последнего до нулевого
            {
                tempInt = array1[i] + array2[i] + extra;//сложение идобавление излишков от 
                //предыдущего сложения
                if(tempInt<10)
                {
                    //Если результатом сложения является число от 0 до 9, это не окажет влияние на 
                    //последующие сложения
                    totalSum.Add(tempInt);
                    extra = 0;
                }
                else
                {
                    //Если в результате сложения получилось двухзначное число, то в массив заносится 
                    //младший регистр(единицы), а при последующем сложении к результату 
                    //добавляется старший регистр (десятки) 
                    totalSum.Add(tempInt % 10);
                    extra = tempInt / 10;
                }
                if (i == 0 & extra > 0)
                {
                    //если в результате сложения последнего элемента получилось двухзначное число
                    //дополнительно дописываем в массив старший регистр (количество десятков)
                    totalSum.Add(extra);
                }
            }
            totalSum.Reverse();
            return totalSum;
        }


        //Метод для умножения массива цифр на цифру. Возвращает массив totalMult.
        public static List<int> MultArrayByDigit(List<int> array, int digit)
        {
            List<int> totalMult = new List<int>();                  
            int tempInt=0, extra=0;
                        
            for(int i=array.Count-1; i>=0; i--)//Цикл перебирает элементы массива с последнего до нулевого
            {
                tempInt = (array[i] * digit)+extra;//Собственно, умножение и добавление излишков
                //от предыдущего умножения (если есть)

                if (tempInt<10) //Внесение результата в массив
                {
                    //Если результатом умножения является число от 0 до 9, это не окажет влияние на последующее 
                    // умножение
                    totalMult.Add(tempInt);                     
                    extra = 0;
                }
                else
                {
                    //Если в результате умножения получилось двухзначное число, то в массив заносится 
                    //младший регистр(единицы), а при последующем умножении к результату 
                    //добавляется старший регистр (десятки)                    
                    totalMult.Add(tempInt%10);
                    extra = tempInt / 10;                    
                }

                if (i==0 & extra>0)                    
                {
                    //если в результате умножения последнего элемента получилось двухзначное число
                    //дополнительно дописываем в массив старший регистр (количество десятков)
                    totalMult.Add(extra);
                }                
            }
            totalMult.Reverse();
            return totalMult;
        }
    }
}
