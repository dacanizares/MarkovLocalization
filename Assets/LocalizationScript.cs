/// AUTHOR:  Daniel Cañizares Corrales
///          Systems Engineer [Professor]
///	     	 Universidad Católica de Oriente
///          dacanizares@outlook.com
///
/// LICENCE: MIT License
/// 	
/// Copyright (C) 2013
///                    
/// Permission is hereby granted, free of charge, to any person obtaining a copy of
/// this software and associated documentation files (the "Software"), to deal in 
/// the Software without restriction, including without limitation the rights to 
/// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
/// of the Software, and to permit persons to whom the Software is furnished to do
/// so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
/// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
/// IN THE SOFTWARE.

using UnityEngine;
using System.Collections;

public class LocalizationScript : MonoBehaviour 
{
	public int worldSize;
	public string[] world;
	private float[] _distribution;
	public float[] Distribution 
	{
		get
		{
			return _distribution;
		}
	}
	public float pHit;
	public float pMiss;
	bool hitting = false;
	int movement;

	void Start () 
	{
		_distribution = new float[worldSize];
		for (int i = 0; i < worldSize; ++i)
		{
			_distribution[i] = 1.0f / worldSize;
		}
	}

	void Update () 
	{
		bool sensing = Input.GetAxis("Vertical") < 0;
		
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, 100)) 
		{
			hitting = true;

			if(!sensing)
				return;
			var tag = hit.transform.gameObject.tag;
			var norm = 0f;
			for(int i = 0; i < worldSize; ++i)
			{
				if (tag == world[i])
					_distribution[i] *= pHit;
				else
					_distribution[i] *= pMiss;				
				norm += _distribution[i];
			}

			for(int i = 0; i < worldSize; ++i)
			{
				_distribution[i] /= norm;
			}

		}
		else
		{
			if(hitting)
			{
				float[] newDistribution = new float[worldSize];
				var norm = 0.0f;
				for(int i = 0; i < worldSize; ++i)
				{
					newDistribution[i] += _distribution[i] * 0.2f;
					newDistribution[(i+1)%worldSize] += _distribution[i] * 0.8f;
					newDistribution[(i+2)%worldSize] += _distribution[i] * 0.2f;
					norm += _distribution[i];
				}

				for(int i = 0; i < worldSize; ++i)
				{
					newDistribution[i] /= norm;
				}
				_distribution = newDistribution;
			}
			hitting = false;
		}

		Debug.DrawRay(transform.position, -Vector3.up);			
	}
}
