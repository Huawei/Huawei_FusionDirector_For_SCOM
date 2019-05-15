#coding=utf-8
import sys
import os
import zipfile
import tarfile

          
def unzip(inputfilepath):
    for filename in os.listdir(inputfilepath):
        # 重命名
        portion = os.path.splitext(filename)
        if portion[1] == ".nupkg":
            newfilename = portion[0] + ".zip"
            filepath = os.path.join(inputfilepath,filename)
            newfilepath = os.path.join(inputfilepath,newfilename)
            os.rename(filepath, newfilepath)
            #解压
            zip_file = zipfile.ZipFile(newfilepath)
            
            newzippath = os.path.join(inputfilepath, portion[0])
            os.mkdir(newzippath)
            
            for file in zip_file.namelist():
                zip_file.extract(file, newzippath)
               
            zip_file.close()                
                
if __name__ == "__main__":
	workspace = sys.argv[1]
	packagepath = os.path.join(workspace, "third_party\\packages")
	unzip(packagepath)
	
