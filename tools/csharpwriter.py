# -*- coding: utf-8 -*- 
# csharpwriter.py
# write csharp file


import os

CSHARP_FILE = 'code/eegametable.cs'

CODE_IMPORT = "using System.Collections;\nusing System.IO;\nusing System.Runtime.InteropServices;\nusing eegame;\n\nnamespace eegametable{\n"
CODE_END = "}\n"
CODE_TAB = "\t"
CODE_CLASS_BEGIN = "\n//%s\n[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]\npublic class %s\n{\n"
CODE_DICCLASS_BEGIN = "\n//%s\n[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi)]\npublic class %s : IDicTable\n{\n\tpublic uint GetKey()\n\t{\n\t\treturn (uint)id;\n\t}\n"
CODE_VAR = "\t[MarshalAsAttribute(UnmanagedType.%s)]\n\tprivate %s %s;\n\tpublic %s %s\n\t{\n\t\tget { return %s; }\n\t\tset { %s = value; }\n\t}\n"
CODE_VAR_STR = "\t[MarshalAsAttribute(UnmanagedType.ByValTStr,SizeConst = %s)]\n\tprivate string %s;\n\tpublic string %s\n\t{\n\t\tget { return %s; }\n\t\tset { %s = value; }\n\t}\n"

class csharpwriter:
    
    def write_code(self,code):
        file_stream = open(CSHARP_FILE,'w')
        file_stream.write(code)
        file_stream.close()
        
    def add_code(self,code):
        file_stream = open(CSHARP_FILE,'a')
        file_stream.write(code)
        file_stream.close()
        
    def write_import(self):
        self.write_code(CODE_IMPORT)

    def add_end(self):
        self.add_code(CODE_END)

    def add_class(self,classinfo):
        tablename = classinfo.table_name
        tableClassname = classinfo.table_classname
        if(classinfo.table_type == 'dic'):
            self.add_code(CODE_DICCLASS_BEGIN%(tablename,tableClassname))
        elif(classinfo.table_type == 'list'):
            self.add_code(CODE_CLASS_BEGIN%(tablename,tableClassname))
            
    def add_class_var(self,format_list,format_name_list):
        var = ''
        for i in range(len(format_list)):
            var += self.add_one_var(format_list[i],format_name_list[i]);
        self.add_code(var)

    def add_one_var(self,var_type,var_name):
        if(var_type.find('string')!=-1):
            str_size = var_type[7:len(var_type)-1]
            return CODE_VAR_STR%(str_size,var_name,var_name.capitalize(),var_name,var_name)
        else:
            enum_type = self.get_unmanagedtype_enum(var_type)
            return CODE_VAR%(enum_type,var_type,var_name,var_type,var_name.capitalize(),var_name,var_name)

    def get_unmanagedtype_enum(self,var_type):

        enum_type = ''
        if(var_type == 'int'):
            enum_type = 'I4'
        elif(var_type == 'float'):
            enum_type = 'R4'
        elif(var_type == 'uint'):
            enum_type = 'U4'
        elif(var_type == 'long'):
            enum_type = 'I8'
        elif(var_type == 'double'):
            enum_type = 'R8'
        elif(var_type == 'ulong'):
            enum_type = 'U8'
        return enum_type
        
       

        


