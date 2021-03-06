﻿using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace GVTool_C_Sharp
{
    class Program
    {
        static void Main(string[] args)
        {            
	    BinaryTree myBST = new BinaryTree();

	    //Console.WriteLine(myBST.Count);
	    for(int i = 0; i<60; i++)
	    {
		Random random = new Random();
		myBST.Insert(random.Next(100));
	    }
	    // myBST.Insert(17);
	    // myBST.Insert(20);
	    // myBST.Insert(13);
	    // myBST.Insert(12);
	    // myBST.Insert(13);
	    // myBST.Insert(12);
	    // myBST.Insert(13);
	    // myBST.Insert(14);
	    
	    Console.WriteLine();

	    myBST.MakeGraphVizFile();
	    
        }
    }
  

    class BNode<T>
    {
	//data
	public int Label { get; set; }
	public T Data { get; set; }
	public BNode<T> LNode { get; set; }	
	public BNode<T> RNode { get; set; }

	
	//operations
	
	//ctor
	public BNode(T someData, int label)
	{
	    Data = someData;
	    Label = label;
	}
    }

    class BinaryTree // : IEnumerable
    {
	
	//data	
	
	public BNode<int> root { get; private set; }
	public int Count { get; private set; }


	
	public void NodeLabelTraversal(FileTool newGraph)
	{
	    NodeLabelTraversal(root, newGraph);
	}

	//inorder LNR adds a graphviz label notation for each node
	public void NodeLabelTraversal(BNode<int> N, FileTool newGraph) 
	{
	    //throw new NotImplementedException();
	    if (N != null)
	    {
		NodeLabelTraversal(N.LNode, newGraph);
		newGraph.Lines.Add($"Node_{N.Label} [label=\"{N.Data}\"]");
		NodeLabelTraversal(N.RNode, newGraph);
	    }
	}


	public void MakeGraphVizFile()
	{
	    FileTool gvFile = new FileTool();

	    //header for GV file ... change to digraph or whatever suites you
	    gvFile.Lines.Add("digraph BinaryTree {");
	    //line space
	    gvFile.Lines.Add("");
	    //assign font
	    gvFile.Lines.Add("node [shape=circle fontname=\"Helvetica\"];");
	    //line space
	    gvFile.Lines.Add("");
	    //call function to add a graphviz label assigning .Data to unique Label of node
	    NodeLabelTraversal(gvFile);
	    //line space
	    gvFile.Lines.Add("");
	    //find max and color nodes red
	    //Max(gvFile);
	    //find min and color nodes yellow
	    Min(gvFile);
	    //line space
	    gvFile.Lines.Add("");	    
	    //call function to stitch all the nodes together
	    TraverseStitchGraph(gvFile);
	    //line space
	    gvFile.Lines.Add("");

	    //end program brace
	    gvFile.Lines.Add("}");
	    //write the actual file change name to suit
	    gvFile.WriteFile("graph.gv");
	    //print file to console cut and paste in:
	    //http://dreampuf.github.io/GraphvizOnline/
	    gvFile.PrintFile("graph.gv");		    	    	    
	}
	
	//traverses and stitches the nodes together in graphviz code
	public void TraverseStitchGraph(FileTool newGraph)
	{
	    TraverseStitchGraph(root, newGraph);
	}

	//inorder LNR (provides sorted values)
	public void TraverseStitchGraph(BNode<int> N, FileTool newGraph) 
	{
	    //throw new NotImplementedException();
	    if(N!=null)
	    {
		//newGraph.Lines.Add($"Node_{N.Label} [label=\"{N.Data}\"]");
		if (N.LNode != null)
		{
		    newGraph.Lines.Add($"Node_{N.Label} -> Node_{N.LNode.Label}");
		    TraverseStitchGraph(N.LNode, newGraph);
		}
		if (N.RNode != null)
		{
		    newGraph.Lines.Add($"Node_{N.Label} -> Node_{N.RNode.Label}");
		    TraverseStitchGraph(N.RNode, newGraph);
		}
	    }	    
	}
	
	//methods for binary tree
	public void Insert(int someData)
	{
	    BNode<int> newNode = new BNode<int>(someData, Count);
	    
	    if(root == null) //tree empty
	    {
		root = newNode;
	    }
	    else
	    {
		BNode<int> finger = root;
		while(true) //finger!=null
		{
		    if(someData <= finger.Data)
		    {
			if(finger.LNode != null)
			    finger = finger.LNode;//move left
			else // no node to left
			{
			    finger.LNode = newNode;
			    break;
			}
		    }
		    else
		    {
			if(finger.RNode != null)
			    finger = finger.RNode;//move right
			else //no node to right
			{
			    finger.RNode = newNode;
			    break;
			}
		    }
		}
	    }
	    Count++;
	}
	
	public BNode<int> Find(int someData)
	{
	    BNode<int> finger = root;

	    while(finger != null && finger.Data != someData)
	    {
	    	if(someData < finger.Data)
	    	    finger = finger.LNode; // move left
	    	else
	    	    finger = finger.RNode; // move right
	    }
	    return finger;
	}
	    
	public void Delete(int someData)
	{
	    throw new NotImplementedException();
	}
	
	public void PreOrderTraversal()
	{
	    PreOrderTraversal(root);
	}

	public void PreOrderTraversal(BNode<int> N)
	{
	    if(N != null)
	    {
		Console.Write(N.Data + " ");
		PreOrderTraversal(N.LNode);
		PreOrderTraversal(N.RNode);
	    }
	}

	public void InOrderTraversal()
	{
	    InOrderTraversal(root);
	}

	//inorder LNR (provides sorted values)
	public void InOrderTraversal(BNode<int> N) 
	{
	    //throw new NotImplementedException();
	    if (N != null)
	    {
		InOrderTraversal(N.LNode);
		Console.Write(N.Data + " ");
		//newGraph.Lines.Add($"Node_{N.Label} [label=\"{N.Data}\"]");
		InOrderTraversal(N.RNode);
	    }
	}

	
	
	public void PostOrderTraversal()
	{
	    PostOrderTraversal(root);
	}

	public void PostOrderTraversal(BNode<int> N)
	{
	    if(N != null)
	    {
		PostOrderTraversal(N.LNode);
		PostOrderTraversal(N.RNode);
		Console.Write(N.Data + " ");
	    }
	}

	public void CountNodes()
	{
	    int count = 0;
	    CountNodes(root, count);
	}

	public int CountNodes(BNode<int> N, int count)
	{
	    if(N != null)
	    {
		CountNodes(N.LNode, count);
		CountNodes(N.RNode, count);
		count++;
	    }
	    return count;
	}

	public int NumberOfLeafNodes(BNode<int> N)
	{
	    if(N == null)
		return 0;
	    else if((N.LNode == null) && (N.RNode == null)) 
		return 1;
	    else
		return NumberOfLeafNodes(N.LNode) + NumberOfLeafNodes(N.RNode);
	}

	// public int Height()
	// {
	//     Height(root);
	// }

	public int Height(BNode<int> N)
	{
	    if(N == null)
		return -1;
	    else
		return 1 + Math.Max(Height(N.LNode), Height(N.RNode));
	}
	
	// public int CountNodez(Nodes<int> N)
	// {
	//     if(N == null)
	// 	return 0;
	//     else
	// 	return 1 + CountNodez(N.LNode) + CountNodez(N.RNode);
	// }

	
	public void Min(FileTool newGraph)
	{
	    //edge
	    if(root == null)
		throw new Exception("No min, empty...");
	    
	    //move left most 
	    BNode<int> finger = root;
	    while(finger.LNode != null)//if ther is a LNode then...
	    {
		newGraph.Lines.Add($"Node_{finger.Label} [style=filled, color=yellow]");	    
		finger = finger.LNode;//
	    }
	    newGraph.Lines.Add($"Node_{finger.Label} [label=\"Min\n{finger.Data}\" , style=filled, color=yellow]");	    
	    //return finger.Data;//

	}

	public void Max(FileTool newGraph)
	{
	    //edge
	    if(root == null)
		throw new Exception("No max, empty...");
	    
	    //move left most 
	    BNode<int> finger = root;
	    while(finger.RNode != null)//if ther is a LNode then..
	    {
		newGraph.Lines.Add($"Node_{finger.Label} [style=filled, color=red]");	    
		finger = finger.RNode;//
	    }
	    newGraph.Lines.Add($"Node_{finger.Label} [label=\"Max\n{finger.Data}\" , style=filled, color=red]");	    
	    //return finger.Data;//
	}
	
	//why don't we need a constructor for generic list?
	//ctor
	public BinaryTree()
	{
	    root = null;
	    Count = 0;
	}
	
    }

}

