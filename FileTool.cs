using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace GVTool_C_Sharp
{
    
    //really cool file handling program that I will probably reuse in this class later on
    class FileTool
    {
    	//data
	public List<string> Lines = new List<string>();//store lines in this list<string> buffer locally


	//method that write all the lines in the local field Lines to a file
	//takes <string> name for file name
    	public void WriteFile(string fileName) //method to write a file
    	{
    	    FileStream stream = null;	
    	    stream = new FileStream(fileName, FileMode.Create);
	    
    	    //Create a streamwriter
    	    using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8 ))
    	    {
    		foreach(string l in Lines)
    		    writer.WriteLine(l); //write each line
    	    }
	    
	    Lines.Clear(); //wipe the lines buffer for the next function to use...
    	}

	
	public void ReadFile(string fileName) //method to read file
	{
	    try
	    {
		var logFile = File.ReadAllLines(fileName); //save resources perhaps by not iterating?
		Lines = new List<string>(logFile);
	    }
	    catch (IOException e)
	    {
		Console.WriteLine("The file could not be read:");
		Console.WriteLine(e.Message);
	    }
	}	

	
	
	public void Reverse(string input, string output)
	{
	    Lines.Clear();//in case there is something in there.
	    ReadFile(input); //read the file into the List line by line

	    DynamicStack<string> newStack = new DynamicStack<string>(); //stack to store lines backward

	    foreach(string l in Lines)//enumerate through the strings in the lists
	    {
		newStack.Push(l);//push the lines onto the stack
	    }

	    Lines.Clear(); //clear the lines buffer

	    int C = newStack.Count; //locally assign count from stack 
	    for(int i = 0; i < C; i++)
	    {
		Lines.Add(newStack.Pop());//add all the lines in reverse from stack
	    }

	    WriteFile(output);//call file write funcion
	}
	
	
	public void PrintFile(string fileName) //print straight from file
	{
	    try
	    {
		// Open the text file using a stream reader.
		using (var str = new StreamReader(fileName))
		{
		    Console.WriteLine(str.ReadToEnd());// Read the stream as a string
		}
	    }
	    catch (IOException e)
	    {
		Console.WriteLine("The file could not be read:");
		Console.WriteLine(e.Message);
	    }
	}
    }

    //generic stack based on ssl
    class DynamicStack<T>
    {
	//add a print in here
	
	//data
	SinglyLinkedList<T> dataList = new SinglyLinkedList<T>();
	public int Count = 0;
	
	//methods
	public void Push(T someData) 
	{
	    dataList.AddFirst(someData);
	    Count++;
	}
	
	public T Pop() //
	{
	    if(!dataList.IsEmpty())
	    {
		T ret = dataList.head.Data;
		dataList.DeleteFirst();
		Count--;
		return ret;
	    }
	    else
		throw new Exception("You can't pop from an empty stack.");
	}
	

	public T Peek() 
	{
	    if (!dataList.IsEmpty())
	    {
		return dataList.head.Data;
	    }
	    else
		throw new Exception("You can't peek at an empty stack.");
	}

	public void Print()
	{
	    Console.WriteLine();//blank line
	    
	    foreach(var elem in dataList) //
		Console.Write(elem + " ");
	    Console.WriteLine();
	}
	
    }
    
    
    //generic node class
    class Node<T> //where T : IComparable<T>
    {
	//data
	public T Data { get; set; }
	public Node<T> Next { get; set; }
	
	//operations
	
	//ctor
	public Node(T someData)
	{
	    Data = someData;
	    Next = null;
	}
    }
    
    //generic ssl
    class SinglyLinkedList<T> : IEnumerable
    {
	//data
	public Node<T> head { get; private set; }
	//public Node<T> Tail { get; private set; }
	
	
	public IEnumerator GetEnumerator()
	{
	    //throw new NotImplementedException();
	    //traverse list
	    Node<T> finger = head;
	    while (finger != null)
	    {
	    	yield return finger.Data;
	    	finger = finger.Next;
	    }
	}
	
	
	
	public void Print()
	{
	    if(IsEmpty())
		Console.WriteLine("List is empty!");
	    else
	    {
		Node<T> finger = head;
		//traverse the list
		while (finger != null)//end after last node
		{
		    Console.Write(finger.Data + " ");
		    finger = finger.Next;// move finger
		}
	    }
	}

	public bool IsEmpty() //
	{
	    //return if null
	    if(head == null)
		return true;
	    else
		return false;
	}

	public void AddFirst(T someData) //
	{
	    Node<T> newNode = new Node<T> (someData); //build new node
	    newNode.Next = head; //newNode points to head
	    head = newNode; //then head points to newNode
	}

	public void AddLast(T someData) //
	{
	    Node<T> newNode = new Node<T>(someData); //build a newNode 

	    if(IsEmpty()) //if new list start the list
	    {
		head = newNode;
	    }
	    else
	    {
		Node<T> finger = head; //create a pointer to navigate
		while(finger.Next != null) //find last node
		{
		    finger = finger.Next;
		}

		finger.Next = newNode; //finger is at last node
	    }
	}

	public void Append(T someData) //
	{
	    AddLast(someData);
	}


	
	public void DeleteFirst() //
	{
	    if(IsEmpty())
		return;
	    else
	    {
		head = head.Next;
	    }
	}
	
	public void Delete(int someData)
	{
	    if(IsEmpty())
	    {
		Console.WriteLine("List is empty.");
	    }
	    else if(head.Data.Equals(someData))
	    {
		head = head.Next;
	    }
	    else
	    {
		Node<T> finger = head;
		while ((finger.Next != null) && (!(finger.Next.Data.Equals(someData)))) 
		{
		    finger = finger.Next;
		}

		if(finger.Next != null)
		{
		    finger.Next = finger.Next.Next;
		}
		else
		{
		    Console.WriteLine($"{someData} not found in the list.");
		}
	    }
	}

	public void Clear()
	{
	    head = null; // garbage monster
	}

	public void Reverse()
	{
	    Node<T> finger = head; // make a pointer
	    head = null; 

	    while (finger != null)
	    {
		AddFirst(finger.Data);
		finger = finger.Next;
	    }
	}

	public bool ContainsACycle()
	{
	    Node<T> slow = head;
	    Node<T> fast = head;

	    while(fast != null && fast.Next != null)
	    {
		slow = slow.Next; //slow moves one
		fast = fast.Next.Next; //fast moves two
		
		if(slow == fast)
		    return true;
	    }
	    return false;
	}
	
    }
}
