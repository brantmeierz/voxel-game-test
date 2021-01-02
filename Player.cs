using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    
    //public Vector3 lastPos;
    public float mouseSensitivity;

    public GameObject mainCamera;
    public float speed;
    public float jumpStrength;
    public float reach;

    public Rigidbody rb;

    public int selectedBlock;

    public Text selectedBlockText;
    public Text targetedBlockText;
    
    void Start() {
        //lastPos = Input.mousePosition;
        mouseSensitivity = 1.0f;

        jumpStrength = 10;
        reach = 5;

        Blocks.LoadBlocks();

        selectedBlock = 1;
        selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";

        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    float RoundIfVeryClose(float input) {
        if (input >= Mathf.Ceil(input) - 0.0001) return Mathf.Ceil(input);
        if (input <= Mathf.Floor(input) + 0.0001) return Mathf.Floor(input);
        return input;
    }

    RaycastHit hit;

    void Update() {

        if (gameObject.transform.position.y < 0)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 128, gameObject.transform.position.z);

        if (Input.GetKeyDown("1")) {
            selectedBlock = 1;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        } else if (Input.GetKeyDown("2")) {
            selectedBlock = 2;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        } else if (Input.GetKeyDown("3")) {
            selectedBlock = 3;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        } else if (Input.GetKeyDown("4")) {
            selectedBlock = 4;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        } else if (Input.GetKeyDown("5")) {
            selectedBlock = 5;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        } else if (Input.GetKeyDown("6")) {
            selectedBlock = 6;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        } else if (Input.GetKeyDown("7")) {
            selectedBlock = 7;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        } else if (Input.GetKeyDown("8")) {
            selectedBlock = 8;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        } else if (Input.GetKeyDown("9")) {
            selectedBlock = 9;
            selectedBlockText.text = "Selected block: " + Blocks.GetBlockName(selectedBlock) + " (" + selectedBlock + ")";
        }

        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, reach)) {
            int targetX = (int)Mathf.Floor(RoundIfVeryClose(hit.point.x));
            int targetY = (int)Mathf.Floor(RoundIfVeryClose(hit.point.y));
            int targetZ = (int)Mathf.Floor(RoundIfVeryClose(hit.point.z));
            targetX = hit.normal.x < 0 ? targetX : (int)(targetX - hit.normal.x);
            targetY = hit.normal.y < 0 ? targetY : (int)(targetY - hit.normal.y);
            targetZ = hit.normal.z < 0 ? targetZ : (int)(targetZ - hit.normal.z);
            targetedBlockText.text = "Targeted block: " + targetX + " " + targetY + " " + targetZ + " (" + Blocks.GetBlockName(WorldLoader.GetBlockAt(targetX, targetY, targetZ)) + ", " + Blocks.NormalToCardinal(hit.normal) + " face)";
        
            if ((Input.GetKey("left shift") && Input.GetMouseButton(0)) || Input.GetMouseButtonDown(0)) {
                WorldLoader.SetBlockAt(targetX, targetY, targetZ, 0); 
            } else if (Input.GetMouseButtonDown(1)) {
                if (!new Vector3((int)gameObject.transform.position.x, (int)gameObject.transform.position.y, (int)gameObject.transform.position.z).Equals(new Vector3(targetX, targetY, targetZ))) {
                    WorldLoader.SetBlockAt((int)(hit.normal.x < 0 ? targetX - 1 : targetX + hit.normal.x), 
                            (int)(hit.normal.y < 0 ? targetY - 1: targetY + hit.normal.y), 
                            (int)(hit.normal.z < 0 ? targetZ - 1: targetZ + hit.normal.z), selectedBlock);
                }
            }
        } else {
            targetedBlockText.text = "Targeted block: None";
        }
        /*if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {

            
                //if (lastHit.normal.Equals(new Vector3(-1.0, 0, 0))) {

                //} else if (lastHit.normal.Equals(new Vector3(0, -1.0, 0)) {

                //}
                int targetX = (int)Mathf.Floor(RoundIfVeryClose(lastHit.point.x));
                int targetY = (int)Mathf.Floor(RoundIfVeryClose(lastHit.point.y));
                int targetZ = (int)Mathf.Floor(RoundIfVeryClose(lastHit.point.z));

                //Removing
                if (Input.GetMouseButtonDown(0)) {
                    WorldLoader.SetBlockAt((int)(lastHit.normal.x < 0 ? targetX : targetX - lastHit.normal.x), 
                        (int)(lastHit.normal.y < 0 ? targetY : targetY - lastHit.normal.y), 
                        (int)(lastHit.normal.z < 0 ? targetZ : targetZ - lastHit.normal.z), 0);
                } else if (Input.GetMouseButtonDown(1)) { //Placing
                    WorldLoader.SetBlockAt((int)(lastHit.normal.x < 0 ? targetX - 1 : targetX), 
                        (int)(lastHit.normal.y < 0 ? targetY - 1: targetY), 
                        (int)(lastHit.normal.z < 0 ? targetZ - 1: targetZ), selectedBlock);
                }
                //WorldLoader.SetBlockAt(targetX, targetY, targetZ, 6);


                //if (lastHit.normal.x == -1) {
                //    WorldLoader.SetBlockAt(targetX + 1, targetY, targetZ, 0);
                //} else if (lastHit.normal.z == -1) {
                //    WorldLoader.SetBlockAt(targetX, targetY, targetZ, 0);
                //} else {
                //    WorldLoader.SetBlockAt(targetX, targetY, targetZ, 0);
                //}

                int chunkX = (int)Mathf.Floor((float)lastHit.point.x / Chunk.WIDTH);
                int chunkZ = (int)Mathf.Floor((float)lastHit.point.z / Chunk.WIDTH);

                WorldLoader.ReloadChunk(chunkX, chunkZ);
                Debug.Log("[Point: " + lastHit.point + "] [Normal: " + lastHit.normal + "] [Target: (" + targetX + ", " + targetY + ", " + targetZ + ")]");
                //Debug.Log("Normal: " + lastHit.normal);
                //Transform objectHit = hit.transform;
                //Debug.DrawLine(hit.point, hit.normal);
            }
        }*/

        if (Input.GetKey("left shift")) {
            speed = 12;
        } else {
            speed = 6;
        }

        //if (lastPos != Input.mousePosition) {
            //Vector3 delta = lastPos - Input.mousePosition;
        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");
            //rb.MoveRotation(Quaternion.Euler(mouseSensitivity * (new Vector3(0, -delta[0], 0))));
        gameObject.transform.Rotate(mouseSensitivity * (new Vector3(0, /*-delta[0]*/deltaX, 0)), Space.Self);
            //if (mainCamera.transform.rotation[0] + mouseSensitivity * /*delta[1]*/deltaY < 90 && mainCamera.transform.rotation[0] + mouseSensitivity * delta[1] > -90)
        if (mainCamera.transform.rotation.x < 90 && mainCamera.transform.rotation.x > -90)
        //if (mainCamera.transform.rotation.x - mouseSensitivity * deltaY < 90 && mainCamera.transform.rotation.x - mouseSensitivity * deltaY > -90)
            mainCamera.transform.Rotate(mouseSensitivity * new Vector3(/*delta[1]*/-deltaY, 0, 0), Space.Self);
            //lastPos = Input.mousePosition;
        //}

        if (Input.GetKey("w")) {
            //rb.MovePosition(rb.position + speed * Vector3.forward * Time.deltaTime);
            gameObject.transform.Translate(speed * Vector3.forward * Time.deltaTime);
        }
        if (Input.GetKey("s")) {
            //rb.MovePosition(rb.position + speed * Vector3.back * Time.deltaTime);
            gameObject.transform.Translate(speed * Vector3.back * Time.deltaTime);
        }
        if (Input.GetKey("a")) {
            //rb.MovePosition(rb.position + speed * Vector3.left * Time.deltaTime);
            gameObject.transform.Translate(speed * Vector3.left * Time.deltaTime);
        }
        if (Input.GetKey("d")) {
            //rb.MovePosition(rb.position + speed * Vector3.right * Time.deltaTime);
            gameObject.transform.Translate(speed * Vector3.right * Time.deltaTime);
        }
        if (Input.GetKeyDown("space")) {
            float dist = GetComponent<Collider>().bounds.extents.y;
            if (Physics.Raycast(transform.position, Vector3.down, dist + 0.1f)) 
                rb.velocity = new Vector3(0, jumpStrength, 0);
            //gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 25);
        }
        //if (Input.GetKey("q")) {
        //    gameObject.transform.Rotate(0, -1, 0, Space.Self);
        //}
        //if (Input.GetKey("e")) {
        //    gameObject.transform.Rotate(0, 1, 0, Space.Self);
        //}
    }
}
