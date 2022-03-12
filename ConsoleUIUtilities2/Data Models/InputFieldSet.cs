using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUIUtilities2
{
    public class InputFieldSet
    {
        private List<Input> m_Inputs = new List<Input>();
        private string m_InputFieldMarker = " "; 

        public void AddInput(Input input, StatusCallback? statusCallback = null)
        {
            m_Inputs.Add(input);
            statusCallback?.Invoke("Input Added"); 
        }

        public void SetInputFieldMarker(string inputFieldMarker)
        {
            m_InputFieldMarker = inputFieldMarker;
        }

        public void RemoveInput(string identifier, StatusCallback? statusCallback = null)
        {
            var value = m_Inputs.Where(i => i.IdentifierID == identifier).FirstOrDefault();
            if (value is not null)
            {
                m_Inputs.Remove(value);
                statusCallback?.Invoke("Input Removed"); 
            }
            else
                statusCallback?.Invoke("Input does not exist"); 
        }


        public void WriteSingleInput(string identifier, int row, int justification, StatusCallback? statusCallback = null, ConsoleColor color = ConsoleColor.White)
        {
            var value = m_Inputs.Where(i => i.IdentifierID == identifier).FirstOrDefault();
            if (value is not null)
            {
                Console.SetCursorPosition(justification, row); 
                Console.ForegroundColor = color;
                Console.Write(value.InputLabel);
                Console.ResetColor(); 
            }
            else
                statusCallback?.Invoke("Input does not exist"); 
        }

        public void WriteMultipleInputs(int startRow, int justification, ConsoleColor color = ConsoleColor.White)
        {
            int rowPosition = startRow;
            int inputFieldMarkerPosition = FindLongestPromptLine(); 

            foreach (Input input in m_Inputs)
            {
                Console.SetCursorPosition(justification, rowPosition); 
                Console.ForegroundColor = color;
                Console.Write(input.InputLabel);
                Console.SetCursorPosition(inputFieldMarkerPosition, rowPosition); 
                Console.Write($" {m_InputFieldMarker} ");
                // Set position of input
                input.SetValueInputPosition(Console.CursorLeft, Console.CursorTop);
                Console.ResetColor(); 
                rowPosition++;
            }
        }

        public void TakeInputValue(string identifier, ConsoleColor inputColor, StatusCallback? statusCallback = null, bool storeUppercase = false)
        {
            var value = m_Inputs.Where(i => i.IdentifierID == identifier).FirstOrDefault();
            if(value is not null)
            {
                Console.SetCursorPosition(value.InputValueStartPositionColumn, value.InputValueStartPositionRow);
                Console.ForegroundColor = inputColor;
                string inputValue = Console.ReadLine() ?? string.Empty;
                if (storeUppercase)
                    inputValue = inputValue.ToUpper(); 
                Console.SetCursorPosition(value.InputValueStartPositionColumn, value.InputValueStartPositionRow);
                Console.Write("".PadRight(inputValue.Length));
                Console.SetCursorPosition(value.InputValueStartPositionColumn, value.InputValueStartPositionRow);
                Console.Write(inputValue);
                value.SetInputValue(inputValue); 
            }
        }

        private int FindLongestPromptLine()
        {
            int length = 0; 
            foreach(Input i in m_Inputs)
            {
                if(i.InputLabel.Length > length)
                {
                    length = i.InputLabel.Length;
                }
            }
            return length; 
        }

        #region Delegates

        /// <summary>
        /// Privides a callback for status messages and errors
        /// </summary>
        /// <param name="status"></param>
        public delegate void StatusCallback(string status);

        #endregion
    }
}
