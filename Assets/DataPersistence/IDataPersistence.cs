using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData data); // N é passado por referência pq o Load só lê os dados
        
    void SaveData(GameData data); // É passado por referência pq os dados carregados podem ser alterados
}
