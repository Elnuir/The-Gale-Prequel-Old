using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossChargingState : AnimatedEntityState
{
   public float ChargeTime;
   private float timer;

   public UnityEvent Charged;

   public override bool IsAvailable => timer > 0;

   public override void ActivateState()
   {
      base.ActivateState();
      if(timer <= 0)
         timer = ChargeTime;
   }

   public override void MakeAvailable()
   {
      base.MakeAvailable();
      timer = ChargeTime;
   }

   public override void DeactivateState()
   {
      base.DeactivateState();
      if(timer <= 0) 
         Charged?.Invoke();
   }

   protected override void Update()
   {
     base.Update();

     if (timer >= 0)
        timer -= Time.deltaTime;
   }
}
