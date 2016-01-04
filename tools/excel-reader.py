# -*- coding: utf-8 -*- 
# excel-reader.py
# read excel tool


import xlrd
import os
import struct
import sys
import byteswriter
import csharpwriter

table_dic = {}
csharpfile = csharpwriter.csharpwriter()

EXCEL_FORMAT_LIST        =      1
EXCEL_FORMAT_NAME_LIST   =      2
EXCEL_REAL_BEGIN         =      3

BYTE_FILE_DIR            =      'Data/'
BYTE_FILE_END            =      '.dat'
EXCEL_FILE_END           =      '.xls'
CONFIG_EXCEL_FILE        =      'docs-all.xls'

                
def read_table(tablestruct,table):
    nrows = table.nrows
    ncols = table.ncols
    format_list = table.row_values(EXCEL_FORMAT_LIST)
    format_name_list = table.row_values(EXCEL_FORMAT_NAME_LIST)
    print "Param Format :",format_list,"   type :",tablestruct.table_type
    print "Write to file :",tablestruct.table_classname+BYTE_FILE_END

    #write csharp file
    csharpfile.add_class(tablestruct)
    csharpfile.add_class_var(format_list,format_name_list)
    csharpfile.add_end()

    #write bytes file
    bytes_str = ''    
    for i in range(EXCEL_REAL_BEGIN,nrows):
        bytes_str += byteswriter.write_bytes(format_list,table.row_values(i))     
    file_stream = open(BYTE_FILE_DIR+tablestruct.table_classname+BYTE_FILE_END,'w')
    file_stream.write(bytes_str)
    file_stream.close()
    
    
def read_file(filename):
    book = xlrd.open_workbook(filename,'r')
    for i in range(book.nsheets):
        if book.sheet_names()[i] in table_dic.keys():
            tablename = book.sheet_names()[i]
            print "Reading table :" ,tablename,table_dic[tablename].table_classname
            read_table(table_dic[tablename],book.sheets()[i])

def read_all_excel():
    csharpfile.write_import()
    dir = os.getcwd()
    for root,dirs,filename in os.walk(dir):
        for file in filename:
            path_filename = os.path.join(root,file)
            if path_filename.endswith(EXCEL_FILE_END):
                print "Reading file :",path_filename
                read_file(path_filename)
    csharpfile.add_end()

def read_config_excel(filename):
    print "Build all excel..."
    book = xlrd.open_workbook(filename)
    table = book.sheets()[0]

    nrows = table.nrows
    for i in range(1,nrows):
        t = byteswriter.excel_table()
        t.table_name = table.row_values(i)[0]
        t.table_classname = table.row_values(i)[1]
        t.table_type = table.row_values(i)[2]
        table_dic[table.row_values(i)[0]] = t

def check_dir():
    if(not (os.path.isdir("Data"))):
        os.mkdir("Data")
    if(not (os.path.isdir("code"))):
        os.mkdir("code")
#def write_bytes_file_test():
#    f = open('test.dat','w')
#    bytes = struct.pack('i',123)
#    f.write(bytes)
#    f.close()

if __name__=="__main__":
    reload(sys)
    sys.setdefaultencoding('utf-8')
    check_dir()
    read_config_excel(CONFIG_EXCEL_FILE)
    read_all_excel()
    
