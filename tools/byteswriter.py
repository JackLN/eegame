# -*- coding: utf-8 -*- 
# byteswriter.py
# write bytes file


import struct


EXCEL_DIC_TABLE = 1


class excel_table:
    def is_dic(self):
        return self.table_type == EXCEL_DIC_TABLE

def write_str(lenght,var):
    #print "string len:",lenght
    param = lenght+'s'
    bytes = struct.pack(param,str(var))
    return bytes

def write_one(var_type,var):
    bytes = ''

    if(var_type.find('string')!=-1):
        bytes = write_str(var_type[7:len(var_type)-1],var)
        return bytes
    
    if(var_type == 'int'):
        bytes = struct.pack('i',int(var))
    elif(var_type == 'float'):
        bytes = struct.pack('f',float(var))
    elif(var_type == 'uint'):
        bytes = struct.pack('l',int(var))
    elif(var_type == 'long'):
        bytes = struct.pack('q',int(var))
    elif(var_type == 'double'):
        bytes = struct.pack('d',float(var))
    elif(var_type == 'ulong'):
        bytes = struct.pack('Q',int(var))
        
    return bytes

def write_bytes(type_list,var_list):
    bytes = ''
    for i in range(len(type_list)):
        bytes += write_one(type_list[i],var_list[i])
    return bytes
