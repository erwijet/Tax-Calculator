using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tax_Calculator
{
    class CommandParser
    {
        public enum SUBROUTINE
        {
            /// <summary>
            ///             
            ///DECLARE: Declares a new 'set' which is a 2 by x matrix
            ///where x is the number of conditions
            ///
            ///it would be visualized as
            ///[
            ///[$5000, 2%]
            ///[$6000, 3%]
            ///[$100000, 10%]
            ///]
            /// 
            ///apon creation of a set, the set will be saved as "<setname>.tc_set"
            ///
            /// SYNTAX:
            /// declare set "<setname> sized <int>"
            /// create a blank set and fills the hashtable with random unique keys
            /// 
            /// declare set "<setname>" as [ $20000 -> 5% $30000 -> 6% $100000 -> 10% ]
            /// declares a filled set with the size as each [cap -> rate] set. The sets do not have commas in between
            /// 
            /// [
            /// [$20000, %5]
            /// [$30000, 6%]
            /// [$100000, 10%]
            /// ]
            /// </summary>
            DECLARE,
            /// <summary>
            /// EDIT: Starts the visual matrix editor
            /// 
            /// SYNTAX:
            /// edit <setname>.tc_set
            /// </summary>
            EDIT,
            /// <summary>
            /// TRUNCATE: Removes the specified set
            /// 
            /// SYNTAX: truncate <setname>.tc_set
            /// </summary>
            TRUNCATE,
            /// <summary>
            /// VIEW: Shows all avalible sets
            /// </summary>
            VIEW,
            /// <summary>
            /// LOAD: Loads a setfile as declares it as a local primary. 
            /// This setfile will be used as the refrence setfile durring calcuation
            /// 
            /// SYNTAX: load <setfile>.tc_set
            /// </summary>
            LOAD,
            /// <summary>
            /// CALCULATE: Finds tax liability, marginal tax rate, and average tax rate of the specified input based on
            /// the input and the loaded setfile
            /// 
            /// SYNTAX:
            /// calc $999999 [-<outputfile>]
            /// if the outputfile flag is waved, then save all output to a new file called <outputfile>.tc_log
            /// </summary>
            CALCULATE,
            SE //SYNTAX ERROR
        }

        public static void DoParse (string commandString)
        {
            string[] wrds = commandString.Split(new char[] { ' ' });
            
            switch (wrds[0])
            {
                case "declare":
                    RateSet rs = new RateSet(
                        new System.Collections.Hashtable());
                    try
                    {
                        rs.DoParse(commandString);
                        rs.SaveMe(rs.Name + ".tc_set");
                    }
                    catch
                    {
                        Console.WriteLine($"@# ERROR: The command { commandString } could not be parsed");
                    }
                    break;
                case "edit":
                    MatrixIndexer index = new MatrixIndexer();
                    RateSet r = new RateSet(
                        new System.Collections.Hashtable());
                    try
                    {
                        r.LoadMe(wrds[1] + ".tc_set");
                        index.PrimaryMatrix = r.GetMatrix();
                        index.SetMatrix(r.GetMatrix().GetLength(1));
                        index.DoUI();
                        r.Rates = index.GetHashtable();
                        System.IO.File.Delete(r.Name + ".tc_set");
                        r.SaveMe(r.Name + ".tc_set");
                    }
                    catch { Console.WriteLine($"@# ERROR: The command { commandString } could not be parsed"); }
                    break;
                case "view":
                    foreach (string thisfile in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.tc_set"))
                    {
                        string file = thisfile.Split(new char[] { '/', '\\' })[thisfile.Split(
                            new char[] { '/', '\\' }).Length - 1]; // Get only the file name and not the file path
                        file = file.Split(new char[] { '.' })[0]; // Remove the .tc_set
                        Console.WriteLine($" # {file}");
                    }
                    break;
            }
        }
    }
}
