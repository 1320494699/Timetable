using System;
using System.Collections.Generic;
using System.Linq;
using QFramework;
using SimpleSQL;
using UnityEngine;

public class DataSaveLoadUtility:IUtility
{
    [Serializable]
    public class StudentDB
    {
        [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
        public int StudentId { get; set; }
        
        public string name { get; set; }
        
        public int grade { get; set; }
        
        public string startTime { get; set; }
        
        public string endTime { get; set; }
        
        public string location { get; set; }
        
        public string weekday { get; set; }
        
        public string classNumber { get; set; }

        public StudentDB()
        {
            
        }
        public StudentDB(StudentData studentData)
        {
            this.name = studentData.name;
            this.grade = studentData.grade;
            this.startTime = studentData.startTime.ToString("yyyy-MM-dd");
            this.endTime = studentData.endTime.ToString("yyyy-MM-dd");
            this.location = studentData.location;
            this.weekday = string.Join(",", studentData.weekday);
            this.classNumber = string.Join(",", studentData.classNumber);
        }
    }
    [Serializable]
    public class TimetableItemDB
    {
        [SimpleSQL.PrimaryKey, SimpleSQL.AutoIncrement]
        public int id { get; set; }
        
        public int studentId { get; set; }
        
        public string time { get; set; }
        
        public int classNumber { get; set; }

        public TimetableItemDB()
        {
            
        }
        public TimetableItemDB(int studentId,  TimetableItemData timetableItemData)
        {
            this.studentId = studentId;
            this.time = timetableItemData.time.ToString("yyyy-MM-dd");
            this.classNumber = timetableItemData.classNumber;
        }
    }
    
    SimpleSQLManager mSqlManager;
    public void SaveData(TimetableItemData timetableItemData)
    {
        CheckHaveTable();
        // 插入数据
        
        StudentDB studentDB = new StudentDB(timetableItemData.studentData);
        long studentId = GetExistingTimetableItemId(studentDB);
        if (studentId>=0)
        {
            Debug.Log("学生数据已存在，更新数据");
        }
        else
        {
            mSqlManager.Insert(studentDB, out studentId);
            Debug.Log("插入学生数据成功，studentId：" + studentId);
        }
        TimetableItemDB timetableItemDB = new TimetableItemDB((int)studentId, timetableItemData);
        mSqlManager.Insert(timetableItemDB);
    }
    private int GetExistingTimetableItemId(StudentDB studentDB)
    {
        bool recordExists = false;
        string query = 
            "SELECT studentId FROM StudentDB WHERE name = ? AND grade = ? AND startTime = ? AND endTime = ? AND location = ? AND weekday = ? AND classNumber = ?";
        StudentDB result = mSqlManager.QueryFirstRecord<StudentDB>(out recordExists, query,
            studentDB.name,
            studentDB.grade,
            studentDB.startTime,
            studentDB.endTime,
            studentDB.location,
            studentDB.weekday,
            studentDB.classNumber);
        if (recordExists)
        {
            return result.StudentId;
        }
        return -1;
    }

    public List<TimetableItemData> LoadData()
    {
        if (CheckHaveTable())
        {
            var timetableItemDBs = new List<TimetableItemDB>(from tidb in mSqlManager.Table<TimetableItemDB>() select tidb);
            var studentDBs = new List<StudentDB>(from sdb in mSqlManager.Table<StudentDB>() select sdb);
            List<TimetableItemData> timetableItemDataList = new List<TimetableItemData>();
            foreach (var item in timetableItemDBs)
            {
                var studentDB = studentDBs.FirstOrDefault(s => s.StudentId == item.studentId);
                if (studentDB == null)
                {
                    Debug.LogError($"item.studentId:{item.studentId} 学生数据不存在");
                    return new List<TimetableItemData>();
                }
                var studentData = new StudentData();
                studentData.name = studentDB.name;
                studentData.grade = studentDB.grade;
                studentData.startTime = studentDB.startTime.ToDateTime();
                studentData.endTime = studentDB.endTime.ToDateTime();
                studentData.location = studentDB.location;
                studentData.weekday = new List<int>();
                studentData.classNumber = new List<int>();
                for (int i = 0; i < studentDB.weekday.Split(',').Length; i++)
                {
                    int weekday =int.Parse(studentDB.weekday.Split(',')[i]);
                    studentData.weekday.Add(weekday);
                }
                
                for (int i = 0; i < studentDB.classNumber.Split(',').Length; i++)
                {
                    int classNumber =int.Parse(studentDB.classNumber.Split(',')[i]);
                    studentData.classNumber.Add(classNumber);
                }
                TimetableItemData timetableItemData = new TimetableItemData();
                timetableItemData.studentData = studentData;
                timetableItemData.time = item.time.ToDateTime();
                timetableItemData.classNumber = item.classNumber;
                timetableItemDataList.Add(timetableItemData);
            }
            return timetableItemDataList;
        }
        Debug.LogError("数据库没有表，请检查");
        return new List<TimetableItemData>();
    }

    public bool CheckHaveTable()
    {
        if (mSqlManager == null)
        {
            mSqlManager = GameObject.FindFirstObjectByType<SimpleSQLManager>();
        }
 
        // 构建查询语句
        string query = "SELECT name FROM sqlite_sequence WHERE name=?";
 
        // 执行查询
        var tableNames = mSqlManager.Query<TimetableItemDB>(query, "TimetableItemDB");
        // 执行查询
        var tableNames1 = mSqlManager.Query<StudentDB>(query, "StudentDB");
        if (tableNames.Count <= 0|| tableNames1.Count <= 0)
        {
            // 创建表
            mSqlManager.CreateTable<TimetableItemDB>();
            mSqlManager.CreateTable<StudentDB>();
            return false;
        }
        return true;
    }
}
