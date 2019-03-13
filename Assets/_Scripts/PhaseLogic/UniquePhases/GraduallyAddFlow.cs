using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraduallyAddFlow : Phase
{
	public float addedFlowPerSecond = 1;

	protected override void PhaseStart()
	{
	}

	protected override void PhaseUpdate()
	{
		flowMan.flow += Time.deltaTime * addedFlowPerSecond;
	}
}
