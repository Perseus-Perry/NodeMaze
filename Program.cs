using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace NodeMaze
{
    class Program
    {

        static string mazeFile = "maze.txt";
        static Stack path = new Stack();
        static void Main(string[] args)
        {
            Console.Write("Enter Starting Node : ");
            int startNode = int.Parse(Console.ReadLine());
            Console.Write("Enter Destination Node : ");
            int destNode = int.Parse(Console.ReadLine());
            Console.WriteLine("Path Found :  "+lookForNode(destNode , startNode));
            if(path.Count != 0)
            {
                Console.Write("Path followed to destination node was : ");
                PrintPath(path);
            }
            Console.Read();
        }

        public static void PrintPath(IEnumerable myCollection)
        {
            foreach (Object obj in myCollection)
                Console.Write(obj + " ");
            Console.WriteLine();
        }

        static bool lookForNode(int lookingFor , int currentNode)
        {
            //go though each child node
            // if node is the one were looking for then return true
            // in the calling func if return true then retturn true
            //else if false then continue with the next item in loop of children
            // if loop ends then return false

            if(lookingFor == currentNode)
            {
                path.Push(currentNode);
                return true;
            }
            else
            {
                //get number of children
                int numberOfChildren = numberOfChildNodes(currentNode);
                //if the above is 0 then this a leaf node. since this aint equal to what we looking for  , return false
                if(numberOfChildren == 0)
                {
                    return false;
                }
                //make array to store children
                int[] children = new int[numberOfChildren];
                //get children if any
                getChildNodes(currentNode).CopyTo(children, 0);
                for(int i = 0; i < numberOfChildren; i++)
                {
                    bool found = false;
                    found = lookForNode(lookingFor, children[i]);
                    if(found)
                    {
                        //add it to a stack
                        path.Push(currentNode);
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }
                //if the loop is over then we didnt find shit
                //return false
                return false;
            }
           
        }

        static int numberOfChildNodes(int node)
        {
            //first fine line number of node info
            string line = null;
            int lineNumber = 0;
            System.IO.StreamReader file = new System.IO.StreamReader(mazeFile);
            while ((line=file.ReadLine())!=null)
            {
                lineNumber = lineNumber + 1;
                if(line.Substring(0,1).Equals(node.ToString()))
                {
                    break;
                }
            }
            // then use string op
            int childrenCount = line.Count(x => x == ',')+1;

            // now check if childnode is not the parent node. this means that there are no child nodes
            if (childrenCount == 1)
            {
                int hyphen = line.IndexOf('-');
                string s = line.Substring(hyphen+1);
                s = s.Trim();
                //Console.WriteLine(s);
                int childnode = int.Parse(s);
                if(childnode < 0)
                {
                    childrenCount = 0;
                }
            }
            return childrenCount;
        }

        static int parentNode(int node)
        {
            //first look for line in which this node is after the hyphen
            string line = null;
            System.IO.StreamReader file = new System.IO.StreamReader(mazeFile);
            while ((line = file.ReadLine()) != null)
            {
                int hyphen_index = line.IndexOf('-');
                int childnode = line.IndexOf(char.Parse(node.ToString()));
                if(hyphen_index < childnode)
                {
                    break;
                }
            }

            string parentNodeChar = line.Substring(0, 1);
            return int.Parse(parentNodeChar);
            //then get the first char of that node
        }

        static int[] getChildNodes(int node)
        {
            int[] childNodes = new int[numberOfChildNodes(node)];
            string line = null;
            System.IO.StreamReader file = new System.IO.StreamReader(mazeFile);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Substring(0, 1).Equals(node.ToString()))
                {
                    break;
                }
            }
            line = line.Substring(line.IndexOf('-') + 1);
         // Console.WriteLine(childNodes.Length);
            if(childNodes.Length == 0)
            {
                return null;
            }
            if (childNodes.Length == 1)
            {
                childNodes[0] = int.Parse(line.Trim());
                return childNodes;
            }
            else if (childNodes.Length > 1)
            {
                for (int i = 0; i < childNodes.Length; i++)
                {
                    int child;
                    if (line.IndexOf(',') >= 0)
                    {
                       child = int.Parse(line.Substring(0, line.IndexOf(',')).Trim());
                      // Console.WriteLine(line.Substring(0, line.IndexOf(',')).Trim());
                    }
                    else
                    {
                        child = int.Parse(line.Trim());
                    }
                    childNodes[i] = child;
                    line = line.Substring(line.IndexOf(',') + 1);
                }
                return childNodes;
            }
            return childNodes;
        }
    }
}
