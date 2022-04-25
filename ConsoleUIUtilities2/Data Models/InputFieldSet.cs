using System.Linq;

namespace ConsoleUIUtilities2
{

    public class InputFieldSet
    {
        private List<Input> m_Inputs = new List<Input>();
        private string m_InputFieldMarker = " ";

        /// <summary>
        /// Represents a set of value entry fields that can be used to collect data
        /// from the user. An array of Input is added to the field set which are customized
        /// for each individual entry field and its location within the console buffer space
        /// </summary>
        public InputFieldSet(string inputFieldMarker = ">> ")
        {
            m_InputFieldMarker = inputFieldMarker;
        }

        /// <summary>
        /// Adds a new input to the field set
        /// </summary>
        /// <param name="input">input to be added</param>
        /// <param name="statusCallback">callback that reports whether or not a field has been added. This is more for debugging and can be left null if it is not desired.</param>
        public void AddInput(Input input, StatusCallback? statusCallback = null)
        {
            m_Inputs.Add(input);
            statusCallback?.Invoke("Input Added");
        }

        /// <summary>
        /// Adds a range of Inputs to the FieldSet. these are accepted as an Input[]
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="statusCallback"></param>
        public void AddInputRange(Input[] inputs, StatusCallback? statusCallback = null)
        {
            foreach (Input input in inputs)
            {
                m_Inputs.Add(input);
                statusCallback?.Invoke("Input Added");
            }
        }

        /// <summary>
        /// Sets the input field marker (>> ) or (-> ) or any marker of your choice. 
        /// It is recommended to add a space after any marker to allow for a clean transition
        /// to the input field. This is an optional component but is set to >> by default. 
        /// </summary>
        /// <param name="marker"></param>
        public void SetInputFieldMarker(string marker = ">> ")
        {
            m_InputFieldMarker = marker;
        }

        /// <summary>
        /// Removes an Input from the FieldSet and sends a callback status if desired.
        /// </summary>
        /// <param name="identifier">Identifier of the input to be removed</param>
        /// <param name="statusCallback">Callback method that matches the delegate signature of StatusCallback</param>
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

        /// <summary>
        /// Writes a single input to the console based on an identifier
        /// NOTE: The input field will be written to the row identified and will be justified
        /// by the amount identified
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="row"></param>
        /// <param name="justification"></param>
        /// <param name="statusCallback"></param>
        /// <param name="color"></param>
        public void WriteSingleInput(string identifier, int row, int justification, StatusCallback? statusCallback = null, ConsoleColor color = ConsoleColor.White)
        {
            var value = m_Inputs.Where(i => i.IdentifierID == identifier).FirstOrDefault();
            if (value is not null)
            {
                Console.SetCursorPosition(justification, row);
                ConsoleBufferSystem.Write(value.InputLabel, color);
            }
            else
                statusCallback?.Invoke("Input does not exist");
        }

        /// <summary>
        /// Writes all input fields in the FieldSet to the console.
        /// NOTE: The fields will be written beginning with the start row and will be justified
        /// from the left side of the console by the amount indicated. 
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="justification"></param>
        /// <param name="color"></param>
        public void WriteMultipleInputs(int startRow, int justification, ConsoleColor color = ConsoleColor.White)
        {
            int currentRowPosition = startRow;
            int longestInputLabel = FindLongestPromptLine();

            foreach (Input item in m_Inputs)
            {
                Console.SetCursorPosition(justification, currentRowPosition);
                ConsoleBufferSystem.Write(item.InputLabel, color);
                Console.SetCursorPosition(longestInputLabel + justification + 1, currentRowPosition);
                ConsoleBufferSystem.Write(m_InputFieldMarker, color);
                item.SetValueInputPosition(Console.CursorLeft, Console.CursorTop);
                currentRowPosition += 1;
            }
        }

        /// <summary>
        /// Begins the input process for all available fields. NOTE: input is accepted on a first in, first out basis
        /// </summary>
        /// <param name="inputColor"></param>
        /// <param name="statusCallback"></param>
        /// <param name="storeAsUppercase"></param>
        public void TakeAllInputValues(ConsoleColor inputColor, StatusCallback? statusCallback = null, bool storeAsUppercase = false)
        {
            foreach (Input input in m_Inputs)
            {
                TakeInputValue(input.IdentifierID, inputColor, statusCallback, storeAsUppercase);
            }
        }

        /// <summary>
        /// Clears the input field and allows the user to enter information again. This input is 
        /// selected by the identifier value
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="valueColor"></param>
        /// <param name="statusCallback"></param>
        public void ClearAndRetakeValue(string identifier, ConsoleColor valueColor = ConsoleColor.White, StatusCallback? statusCallback = null, bool storeUppercase = false)
        {
            var result = m_Inputs.Where(i => i.IdentifierID == identifier).FirstOrDefault();
            if (result is not null)
            {
                Console.SetCursorPosition(result.InputValueStartPositionColumn, result.InputValueStartPositionRow);
                ConsoleBufferSystem.Write("".PadRight(result.InputValue.Length));
                TakeInputValue(identifier, valueColor, statusCallback, storeUppercase);
            }
        }

        /// <summary>
        /// Clears and rewrites the value to the console. This is useful if you need to regenerate
        /// the contents of a console page or refresh certain data as it is updated externally. 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="valueColor"></param>
        public void ClearAndRewriteValue(string identifier, ConsoleColor valueColor = ConsoleColor.White)
        {
            var result = m_Inputs.Where(i => i.IdentifierID == identifier).FirstOrDefault();
            if (result is not null)
            {
                Console.SetCursorPosition(result.InputValueStartPositionColumn, result.InputValueStartPositionRow);
                ConsoleBufferSystem.Write("".PadRight(result.InputValue.Length));
                Console.SetCursorPosition(result.InputValueStartPositionColumn, result.InputValueStartPositionRow);
                ConsoleBufferSystem.Write(result.InputValue);
            }
        }

        /// <summary>
        /// Takes in the value for the input field. 
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="inputColor"></param>
        /// <param name="statusCallback"></param>
        /// <param name="storeUppercase"></param>
        public void TakeInputValue(string identifier, ConsoleColor inputColor, StatusCallback? statusCallback = null, bool storeUppercase = false)
        {
            var value = m_Inputs.Where(i => i.IdentifierID == identifier).FirstOrDefault();
            if (value is not null)
            {
                Console.SetCursorPosition(value.InputValueStartPositionColumn, value.InputValueStartPositionRow);
                string inputValue = Console.ReadLine() ?? string.Empty;
                if (storeUppercase)
                    inputValue = inputValue.ToUpper();
                Console.SetCursorPosition(value.InputValueStartPositionColumn, value.InputValueStartPositionRow);
                Console.Write("".PadRight(inputValue.Length), inputColor);
                Console.SetCursorPosition(value.InputValueStartPositionColumn, value.InputValueStartPositionRow);
                ConsoleBufferSystem.Write(inputValue, inputColor);
                value.SetInputValue(inputValue);
                statusCallback?.Invoke($"VALUE ACCEPTED: {inputValue}");
            }
            else
                statusCallback?.Invoke($"VALUE NOT ACCEPTED: INPUT WAS NULL");
        }

        /// <summary>
        /// Finds the longest label in the FieldSet
        /// NOTE: This is used to find the longest label so that input areas can be set to begin
        /// at the same location when writing multiple inputs simultaneously. 
        /// </summary>
        /// <returns></returns>
        private int FindLongestPromptLine()
        {
            int length = 0;
            foreach (Input i in m_Inputs)
            {
                if (i.InputLabel.Length > length)
                {
                    length = i.InputLabel.Length;
                }
            }
            return length;
        }

        /// <summary>
        /// Gets the value from an input field based upon the provided identifierID
        /// </summary>
        /// <param name="identifierID"></param>
        /// <returns></returns>
        public string GetValue(string identifierID)
        {
            var result = m_Inputs.Where(i => i.IdentifierID == identifierID).FirstOrDefault();
            if (result is not null)
            {
                return result.InputValue;
            }
            return string.Empty;
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
