#!/usr/bin/env node
const fs = require("fs");
const path = require("path");

// 1st 2 names are of interpreter in use and package path
let args = process.argv.slice(2)
// console.log(process.argv.slice(2));
if(args.length>=2){
    try{
        let filepath=args[0]
        let fileExt=args.slice(1);
        let fileName=path.basename(filepath)

        if(filepath && fileExt && fileName){
            
            fileExt.forEach(ext=>{
                createFile(filepath,'/'+fileName.toLowerCase()+'.'+ext);
            })

        }else{
            console.log("Something is wrong with arguments passed");
        }


    }catch(error){
        console.log("SOMETHING WENT WRONG\n",error);
    }
}else{
    showHelp()
}


function showHelp(){
    console.log(`
Welcome to CreateFiles

** Easily Generate files within a folder **

USAGE:

createfiles <PATH> <FILE EXTENTIONS>

EXAMPLE:
#createfiles /src/Home html css js
`);
}

async function createFile (filepath,fileName){
    // Check if the directory exists.
    if (!fs.existsSync(filepath)) {
      // The directory does not exist. Create the directory recursively.
      await fs.promises.mkdir(filepath, { recursive: true });
    }
  
    // Create the file.
    await fs.promises.writeFile(filepath+fileName, "").then((value)=>{
        console.log("Created "+fileName);
    });
  };