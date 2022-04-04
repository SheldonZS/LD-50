using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    private int health;
    public int maxHealth;

    public int level = 1;
    public int exp = 0;
    public int maxExp = 5;

    public float moveSpeed = 200f;
    public float buildSpeedMultiplier = 1f;

    public GameObject[] towers;
    public GameObject[] attackPrefabs;

    private HealthBar healthBar;
    private List<HeroCommand> commands;
    private Animator animator;
    private Animator highlight;
    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private RTSController RTSC;

    private float repairDecimal;
    private TowerBase repairTower;

    public bool upgrading;
    private float upgradeStartTime;

    public bool building;
    public bool repairing;

    private bool firstBuild, firstUpgrade, firstRuin, leveledUp, firstHurt;

    private int bonesCarried = 0;

    private DataBucket db;
    private DialogueBox diaBox;
    private DialogueManager DM;
    private Text boneCountText;
    private SpriteRenderer boneIcon;

    public float bardBuildMultiplier = 1f;
    public float bardMoveMultiplier = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();

        RTSC = GetComponentInParent<RTSController>();

        commands = new List<HeroCommand>();
        health = maxHealth;
        healthBar = GetComponentInChildren<HealthBar>();

        healthBar.UpdateHealth(health, maxHealth);

        highlight = transform.GetChild(1).GetComponent<Animator>();
        highlight.SetBool("Selected", false);

        db = GameObject.Find("DataBucket").GetComponent<DataBucket>();
        diaBox = GameObject.Find("TextWindow").GetComponent<DialogueBox>();
        DM = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        boneIcon = transform.GetChild(2).GetComponent<SpriteRenderer>();
        boneCountText = transform.GetChild(3).GetChild(0).GetComponent<Text>();

        UpdateBoneUI();
    }

    // Update is called once per frame
    void Update()
    {
        //

   

        if ((db.tutorialMode >=2 && this.name == "Raol") || db.tutorialMode >= 4)
        {

            if (commands.Count == 0) commands.Add(new HeroCommand(Commands.idle));
            HeroCommand currentCommand = commands[0];

            if (RTSC.selected != gameObject)
            {
                highlight.SetBool("Selected", false);

                ExecuteCommand();



                return;
            }

            if (collider.IsTouching(RTSC.homeBase))
            {
                RTSC.bones += bonesCarried;
                bonesCarried = 0;
                RTSC.UpdateButtons();
                UpdateBoneUI();


            }

            highlight.SetBool("Selected", true);
            bool manualMove = false;
            if ((currentCommand.command == Commands.idle || currentCommand.command == Commands.move) &&
            (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow)))
            {
                if (!building && !upgrading)
                {
                    if (db.tutorialMode == 2)
                    {
                        ShowText(DM.tutorial2, TextMode.imm);
                        db.tutorialMode++;
                    }
                    commands.Clear();
                    commands.Add(new HeroCommand(Commands.idle));

                    Vector2 direction = Vector2.zero;

                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) direction.y += 1;
                    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) direction.x -= 1;
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) direction.y -= 1;
                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) direction.x += 1;

                    rb.velocity = direction.normalized * moveSpeed * bardMoveMultiplier;
                    manualMove = true;
                }

                
            }
            else if (Input.GetMouseButtonDown(1) && !building && !upgrading)
            {
                if (db.tutorialMode == 2)
                {
                    ShowText(DM.tutorial2, TextMode.imm);
                    db.tutorialMode++;
                }
                Vector2 clickPos = RTSC.MouseToGrid();
                //RTSC.Say(this, "Right Clicked at world position " + clickPos);

                if (clickPos.x >= -.5 && clickPos.x <= 17.5 && clickPos.y >= 0 && clickPos.y <= 10)
                {
                    if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                    {
                        commands.Clear();
                        rb.velocity = Vector2.zero;
                    }

                    commands.Add(new HeroCommand(Commands.move, clickPos));
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Vector2 clickPos = RTSC.MouseToGrid();

                if (clickPos.x >= -.5 && clickPos.x <= 17.5 && clickPos.y >= 0 && clickPos.y <= 10)
                {

                    switch (RTSC.command)
                    {
                        case Commands.build:
                            if (RTSC.GridContains(RTSC.MouseToGrid(), "Monster"))
                                RTSC.Say(this, "There are monsters there");
                            if (RTSC.GridContains(RTSC.MouseToGrid(), "Tower"))
                                RTSC.Say(this, "There's already a tower there");
                            else
                            {
                                if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                                    commands.Clear();
                                commands.Add(new HeroCommand(Commands.build, RTSC.RoundToGrid(RTSC.MouseToGrid()), towers[0]));
                            }
                            break;
                        case Commands.repair:
                            TowerBase targetTower = RTSC.GetTowerAt(clickPos);
                            if (targetTower == null)
                                RTSC.Say(this, "No tower here");
                            else if (targetTower.builder != this)
                                RTSC.Say(this, "This is not my tower");
                            else
                            {
                                if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                                    commands.Clear();
                                commands.Add(new HeroCommand(Commands.repair, clickPos, targetTower.gameObject));
                            }
                            break;
                        case Commands.upgrade:
                            targetTower = RTSC.GetTowerAt(clickPos);
                            if (targetTower == null)
                                RTSC.Say(this, "No tower here");
                            else if (targetTower.builder != this)
                                RTSC.Say(this, "This is not my tower");
                            else
                            {
                                if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                                    commands.Clear();
                                commands.Add(new HeroCommand(Commands.upgrade, clickPos, targetTower.gameObject));
                            }
                            break;
                        case Commands.move:
                        default:
                            /*
                            Vector3 mouseWorldPosition = RTSC.camera.ScreenToWorldPoint(Input.mousePosition);

                            //RTSC.Say(this, "Clicked at world position: " + mouseWorldPosition);

                            RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPosition, Vector2.zero);

                            bool canMove = true;
                            foreach (RaycastHit2D hit in hits)
                            {
                                if (hit.collider.gameObject.tag == "Player")
                                {
                                    if (hit.collider.gameObject != gameObject)
                                    {
                                        canMove = false;
                                    }
                                }
                            }

                            if (canMove)
                            {
                                if (currentCommand.command == Commands.idle || (!(Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))))
                                {
                                    commands.Clear();
                                    rb.velocity = Vector2.zero;
                                }

                                commands.Add(new HeroCommand(Commands.move, clickPos));
                            }
                            */
                            break;
                    }
                }
            }

            if (!manualMove)
                ExecuteCommand();
        }
        

    }

    public void PickUpBones(int x)
    {
        bonesCarried += x;
        UpdateBoneUI();

        switch (this.name)
        {
            case "Raol":
                DM.PlayRandom(DM.bonePickUp_R);
                break;
            case "Balthasar":
                DM.PlayRandom(DM.bonePickUp_B);
                break;
            case "Thob":
                DM.PlayRandom(DM.bonePickUp_T);
                break;
            case "Jolie":
                DM.PlayRandom(DM.bonePickUp_J);
                break;
            default:
                break;
        }

    }

    private void ExecuteCommand()
    {
        if (commands.Count == 0)
            commands.Add(new HeroCommand(Commands.idle));

        if (executeCommand(commands[0]))
            commands.RemoveAt(0);
    }
    private bool executeCommand(HeroCommand command)
    {
        switch (command.command)
        {
            case Commands.move:
                Vector2 movement = command.location - (Vector2)transform.localPosition;
                float mag = movement.magnitude;

                if (mag <= moveSpeed * bardMoveMultiplier * Mathf.Max(Time.deltaTime, Time.fixedDeltaTime) || Vector2.Angle(rb.velocity, movement) > 90)
                {
                    transform.localPosition = command.location;
                    rb.velocity = Vector2.zero;
                    return true;
                }
                else
                {
                    rb.velocity = movement.normalized * moveSpeed * bardMoveMultiplier;
                    return false;
                }

            case Commands.build:
                //check if player is touching the tower build location
                rb.velocity = Vector2.zero;
                Collider2D[] hits = Physics2D.OverlapBoxAll(RTSC.gridAnchor.TransformPoint(RTSC.RoundToGrid(command.location, 0.5f)), Vector2.one * 0.9f, 0);

                bool inPosition = false;
                foreach (Collider2D hit in hits)
                {
                    if (hit.gameObject == gameObject)
                    {
                        inPosition = true;
                        break;
                    }
                }

                if (inPosition)
                {
                    if (RTSC.GridContains(command.location, "Monster"))
                    {
                        //RTSC.Say(this, "Monsters Here");
                        return true;
                    }
                    else if (RTSC.GridContains(command.location, "Tower"))
                    {
                        //RTSC.Say(this, "Already a tower here");
                        return true;
                    }
                    else if (RTSC.GridContains(command.location, "Path"))
                    {
                       // RTSC.Say(this, "Path Here");
                        return true;
                    }
                    else if (RTSC.GridContains(command.location, "Obstacle"))
                    {
                       // RTSC.Say(this, "Obstacle Here");
                        return true;
                    }

                    building = true;
                    RTSC.UpdateButtons();

                    TowerBase newTower = Instantiate(towers[0], RTSC.gridAnchor).GetComponent<TowerBase>();
                    newTower.transform.localPosition = command.location;
                    newTower.builder = this;
                    newTower.SetHealth(1);

                    commands[0] = new HeroCommand(Commands.buildToMax, command.location, newTower.gameObject);

                    RTSC.Reset();
                    return false;
                }
                else
                {
                    Vector2 direction = command.location - (Vector2)transform.localPosition;
                    rb.velocity = direction.normalized * moveSpeed * bardMoveMultiplier;
                    return false;
                }

            case Commands.buildToMax:
                rb.velocity = Vector2.zero;
                repairTower = command.tower.GetComponent<TowerBase>();
                if (repairTower == null || repairTower.health <= 0)
                {
                    //RTSC.Say(this, "No tower here");
                    repairDecimal = 0;
                    return true;
                }

                if (collider.IsTouching(repairTower.GetComponent<BoxCollider2D>()))
                {
                    float repairAmount = (repairTower.maxHealth / repairTower.buildTime) * buildSpeedMultiplier * bardBuildMultiplier * Time.deltaTime + repairDecimal;
                    int repairInt = Mathf.FloorToInt(repairAmount);
                    repairTower.Repair(repairInt);

                    if (repairTower.health >= repairTower.maxHealth)
                    {
                        
                        repairDecimal = 0;
                        building = false;
                        RTSC.UpdateButtons();
                        exp++;
                        exp = Mathf.Clamp(exp, 0, maxExp);
                        //update exp bar
                        if (exp == 5)
                        {
                            level = 2;
                            if (!leveledUp)
                            {
                                leveledUp = true;
                                switch (this.name)
                                {
                                    case "Raol":
                                        ShowText(DM.levelUp_R, TextMode.ifFree);

                                        break;
                                    case "Balthasar":
                                        ShowText(DM.levelUp_B, TextMode.ifFree);

                                        break;
                                    case "Thob":
                                        ShowText(DM.levelUp_T, TextMode.ifFree);

                                        break;
                                    case "Jolie":
                                        ShowText(DM.levelUp_J, TextMode.ifFree);

                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        if (db.tutorialMode == 3)
                        {
                            ShowText(DM.tutorial3, TextMode.imm);
                            db.tutorialMode++;
                        }

                        //if not built before, play special text. otherwise, pick random text
                        if (!firstBuild)
                        {
                            firstBuild = true;
                            switch (this.name)
                            {
                                case "Raol":

                                    break;
                                case "Balthasar":
                                    ShowText(DM.firstTower_B, TextMode.ifFree);

                                    break;
                                case "Thob":
                                    ShowText(DM.firstTower_T, TextMode.ifFree);

                                    break;
                                case "Jolie":
                                    ShowText(DM.firstTower_J, TextMode.ifFree);

                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (this.name)
                            {
                                case "Raol":
                                    DM.PlayRandom(DM.towerBuilt_R);
                                    break;
                                case "Balthasar":
                                    DM.PlayRandom(DM.towerBuilt_B);
                                    break;
                                case "Thob":
                                    DM.PlayRandom(DM.towerBuilt_T);
                                    break;
                                case "Jolie":
                                    DM.PlayRandom(DM.towerBuilt_J);
                                    break;
                                default:
                                    break;
                            }
                        }

                        return true;
                    }

                    repairDecimal = repairAmount - repairInt;
                }
                else if (!building)
                {
                    Vector2 direction = command.location - (Vector2)transform.localPosition;
                    rb.velocity = direction.normalized * moveSpeed * bardMoveMultiplier;
                }
                return false;

            case Commands.repair:
                rb.velocity = Vector2.zero;
                repairTower = command.tower.GetComponent<TowerBase>();
                if (repairTower == null || repairTower.health <= 0)
                {
                   // RTSC.Say(this, "No tower here");
                    repairDecimal = 0;
                    return true;
                }

                if (collider.IsTouching(repairTower.GetComponent<BoxCollider2D>()))
                {
                    repairing = true;
                    RTSC.UpdateButtons();

                    float repairAmount = (repairTower.maxHealth / repairTower.buildTime) * buildSpeedMultiplier * bardBuildMultiplier * Time.deltaTime + repairDecimal;
                    int repairInt = Mathf.FloorToInt(repairAmount);
                    repairTower.Repair(repairInt);

                    if (repairTower.health >= repairTower.maxHealth)
                    {
                       // RTSC.Say(this, "Job's done!");
                        repairDecimal = 0;
                        repairing = false;
                        RTSC.UpdateButtons();

                        switch (this.name)
                        {
                            case "Raol":
                                DM.PlayRandom(DM.towerFullRepair_R);
                                break;
                            case "Balthasar":
                                DM.PlayRandom(DM.towerFullRepair_B);
                                break;
                            case "Thob":
                                DM.PlayRandom(DM.towerFullRepair_T);
                                break;
                            case "Jolie":
                                DM.PlayRandom(DM.towerFullRepair_J);
                                break;
                            default:
                                break;
                        }
                        return true;
                    }

                    repairDecimal = repairAmount - repairInt;
                }
                else
                {
                    Vector2 direction = command.location - (Vector2)transform.localPosition;
                    rb.velocity = direction.normalized * moveSpeed * bardMoveMultiplier;
                }
                return false;

            case Commands.upgrade:
                rb.velocity = Vector2.zero;
                repairTower = command.tower.GetComponent<TowerBase>();
                if (repairTower == null || repairTower.health <= 0)
                {
                    //RTSC.Say(this, "No tower here");
                    upgrading = false;
                    return true;
                }

                if (collider.IsTouching(repairTower.GetComponent<BoxCollider2D>()))
                {
                    if (upgrading == false)
                    {

                        if (repairTower.health < repairTower.maxHealth * 0.5f)
                        {
                            //RTSC.Say(this, "This tower is too badly damaged to upgrade");
                            return true;
                        }
                        
                        if (repairTower.operational == false)
                        {
                            //RTSC.Say(this, "Finish building this before upgrading");
                            return true;
                        }

                        upgrading = true;
                        RTSC.UpdateButtons();

                        upgradeStartTime = Time.time;

                        RTSC.Say(this, "Starting upgrade...");
                        return false;
                    }
                    else
                    {
                        if (Time.time >= upgradeStartTime + repairTower.upgradeTime)
                        {
                            repairTower.Upgrade();
                            //RTSC.Say(this, "Job's done!");
                            upgrading = false;
                            RTSC.UpdateButtons();
                            
                            //if not upgraded before, play special text. otherwise, pick random
                            if (!firstUpgrade)
                            {
                                firstUpgrade = true;
                                switch (this.name)
                                {
                                    case "Raol":
                                        ShowText(DM.firstUpgrade_R, TextMode.ifFree);

                                        break;
                                    case "Balthasar":
                                        ShowText(DM.firstUpgrade_B, TextMode.ifFree);

                                        break;
                                    case "Thob":
                                        ShowText(DM.firstUpgrade_T, TextMode.ifFree);

                                        break;
                                    case "Jolie":
                                        ShowText(DM.firstUpgrade_J, TextMode.ifFree);

                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                switch (this.name)
                                {
                                    case "Raol":
                                        DM.PlayRandom(DM.towerUpgrade_R);
                                        break;
                                    case "Balthasar":
                                        DM.PlayRandom(DM.towerUpgrade_B);
                                        break;
                                    case "Thob":
                                        DM.PlayRandom(DM.towerUpgrade_T);
                                        break;
                                    case "Jolie":
                                        DM.PlayRandom(DM.towerUpgrade_J);
                                        break;
                                    default:
                                        break;
                                }
                            }

                            return true;
                        }
                    }

                }
                else if (!upgrading)
                {
                    Vector2 direction = command.location - (Vector2)transform.localPosition;
                    rb.velocity = direction.normalized * moveSpeed * bardMoveMultiplier;
                }
                return false;
            case Commands.idle:
            default:
                rb.velocity = Vector2.zero;
                return false;
        }

    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.UpdateHealth(health);

        //play damage sound

        if (!firstHurt)
        {
            firstHurt = true;
            switch (this.name)
            {
                case "Raol":
                    ShowText(DM.firstHurt_R, TextMode.ifFree);

                    break;
                case "Balthasar":
                    ShowText(DM.firstHurt_B, TextMode.ifFree);

                    break;
                case "Thob":
                    ShowText(DM.firstHurt_T, TextMode.ifFree);

                    break;
                case "Jolie":
                    ShowText(DM.firstHurt_J, TextMode.ifFree);

                    break;
                default:
                    break;
            }
        }
        if (health <= 0)
        {

            CharacterDeath();

        }
    }

    void CharacterDeath()
    {
        int lifeCount = 0;
        Time.timeScale = 0;

        foreach (GameObject hero in RTSC.heroes)
        {
            if (hero != null)
                lifeCount++;
        }
        switch (this.name)
        {
            case "Raol":
                //play character death anim
                //play character death sound

                switch (lifeCount)
                {
                    case 4:
                        ShowText(DM.firstDeath_R, TextMode.imm);
                        break;
                    case 3:
                        ShowText(DM.secondDeath_R, TextMode.imm);
                        break;
                    case 2:
                        ShowText(DM.thirdDeath_R, TextMode.imm);
                        break;
                    case 1:
                        ShowText(DM.lastDeath_R, TextMode.imm);
                        db.data.ending = EndingCode.Raol;
                        db.raolUnlocked = true;
                        break;
                }
                break;
            case "Balthasar":
                //play character death anim
                //play character death sound

                switch (lifeCount)
                {
                    case 4:
                        ShowText(DM.firstDeath_B, TextMode.imm);
                        break;
                    case 3:
                        ShowText(DM.secondDeath_B, TextMode.imm);
                        break;
                    case 2:
                        ShowText(DM.thirdDeath_B, TextMode.imm);
                        break;
                    case 1:
                        ShowText(DM.lastDeath_B, TextMode.imm);
                        db.data.ending = EndingCode.Bal;
                        db.balUnlocked = true;
                        break;
                }
                break;
            case "Thob":
                //play character death anim
                //play character death sound

                switch (lifeCount)
                {
                    case 4:
                        ShowText(DM.firstDeath_T, TextMode.imm);
                        break;
                    case 3:
                        ShowText(DM.secondDeath_T, TextMode.imm);
                        break;
                    case 2:
                        ShowText(DM.thirdDeath_T, TextMode.imm);
                        break;
                    case 1:
                        ShowText(DM.lastDeath_T, TextMode.imm);
                        db.data.ending = EndingCode.Thob;
                        db.thobUnlocked = true;
                        break;
                }
                break;
            case "Jolie":
                //play character death anim
                //play character death sound

                switch (lifeCount)
                {
                    case 4:
                        ShowText(DM.firstDeath_J, TextMode.imm);
                        break;
                    case 3:
                        ShowText(DM.secondDeath_J, TextMode.imm);
                        break;
                    case 2:
                        ShowText(DM.thirdDeath_J, TextMode.imm);
                        break;
                    case 1:
                        ShowText(DM.lastDeath_J, TextMode.imm);
                        db.data.ending = EndingCode.Jolie;
                        db.jolieUnlocked = true;
                        break;
                }
                break;
           
        }//switch depending on character that is dying
    }

    void UpdateBoneUI()
    {
        if (bonesCarried == 0)
        {
            boneIcon.enabled = false;
            boneCountText.enabled = false;
        }
        else
        {
            boneIcon.enabled = true;
            boneCountText.text = bonesCarried.ToString();
            boneCountText.enabled = true;
        }
    }
    void ShowText(List<string> storyText, TextMode mode)
    {
        StartCoroutine(diaBox.PlayText(storyText, mode));
    }

    public void PlayRuinText()
    {
        if (!firstRuin)
        {
            firstRuin = true;
            switch (this.name)
            {
                case "Raol":
                    ShowText(DM.firstRuin_R, TextMode.ifFree);

                    break;
                case "Balthasar":
                    ShowText(DM.firstRuin_B, TextMode.ifFree);

                    break;
                case "Thob":
                    ShowText(DM.firstRuin_T, TextMode.ifFree);

                    break;
                case "Jolie":
                    ShowText(DM.firstRuin_J, TextMode.ifFree);

                    break;
                default:
                    break;
            }
        }
    }
}

[System.Serializable]
public class HeroCommand
{
    public Commands command = Commands.idle;
    public Vector2 location;
    public GameObject tower;

    public HeroCommand(Commands com)
    {
        command = com;
        tower = null;
    }
    public HeroCommand(Commands com, Vector2 dest, GameObject target = null)
    {
        command = com;
        location = dest;
        tower = target;
    }
}

public enum Commands { idle, move, build, repair, upgrade, buildToMax}
public enum CharacterAnimation { idle, move, action, hurt, dying} //action used for Heros building or monsters attacking