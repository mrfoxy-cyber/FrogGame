using System;
using UnityEngine;

public class Cloud : MonoBehaviour
{
  // Start is called before the first frame update
  public float speed;
  public float movetox;
  public float movetoy;
  public bool moving;
  private Vector2 startpos;

  void Start()
  {
    moving = false;
    startpos.x = transform.position.x;
    startpos.y = transform.position.y;
  }

  // Update is called once per frame
  void Update()
  {
    MoveTo(movetox, movetoy);
  }

  public void Eat(Parameters.FrogTypes froggy)
  {
    if (froggy == Parameters.FrogTypes.Empty)
      this.GetComponent<Animator>().Play("cloudeat");
    else
      this.GetComponent<Animator>().Play("cloudeatfroggy");
  }

  public void Move()
  {
    this.GetComponent<Animator>().Play("fly");
  }

  public void CloudCarryFroggy()
  {
    this.GetComponent<Animator>().Play("carryFroggy");
  }

  public void Home()
  {
    movetox = startpos.x;
    movetoy = startpos.y;
  }

  private void MoveTo(float x, float y)
  {
    float xtmp = this.transform.position.x;
    float ytmp = this.transform.position.y;
    float step = speed * Time.deltaTime;

    if (xtmp != x)
    {
      moving = true;
      if (xtmp < x)
        xtmp = xtmp + step;
      else if (xtmp > x)
        xtmp = xtmp - step;
    }
    //becasue to define the movement first for y axixs, then for x
    if ((ytmp != y) && (Math.Abs(x - xtmp) <= step))
    {
      moving = true;
      if (ytmp < y)
        ytmp = ytmp + step;
      else if (ytmp > y)
        ytmp = ytmp - step;
    }

    //should take care of the last step rounding
    if ((Math.Abs(y - ytmp) <= step))
    {
      moving = false;
      xtmp = x;
      ytmp = y;
    }

    transform.position = new Vector3(xtmp, ytmp, 0);

  }

  internal Vector2 ToGrid()
  {
    //  int x = UnityEngine.Random.Range(0, GridManager.Instance.instantiatedGridSlots.GetLength(0));
    //  int y = UnityEngine.Random.Range(0, GridManager.Instance.instantiatedGridSlots.GetLength(1));
    // GridManager.Instance.instantiatedGridSlots[x, y].CallCloud();
    return new Vector2(1, 1);
  }
}
