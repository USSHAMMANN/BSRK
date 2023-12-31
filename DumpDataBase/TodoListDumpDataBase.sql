PGDMP      9                {            base    16.0    16.0     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    24657    base    DATABASE     x   CREATE DATABASE base WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE base;
                usssims    false                        2615    24658 	   TodoShema    SCHEMA        CREATE SCHEMA "TodoShema";
    DROP SCHEMA "TodoShema";
                usssims    false            �            1259    24659    Task    TABLE     �   CREATE TABLE "TodoShema"."Task" (
    "Id" bigint NOT NULL,
    "UserId" bigint NOT NULL,
    "Description" text NOT NULL,
    "Timeframe" timestamp without time zone NOT NULL,
    "Priority" text NOT NULL,
    "Done" boolean NOT NULL
);
    DROP TABLE "TodoShema"."Task";
    	   TodoShema         heap    usssims    false    5            �            1259    24664    User    TABLE     ~   CREATE TABLE "TodoShema"."User" (
    "Id" bigint NOT NULL,
    "UserName" text NOT NULL,
    "PasswordHash" text NOT NULL
);
    DROP TABLE "TodoShema"."User";
    	   TodoShema         heap    usssims    false    5            �          0    24659    Task 
   TABLE DATA           e   COPY "TodoShema"."Task" ("Id", "UserId", "Description", "Timeframe", "Priority", "Done") FROM stdin;
 	   TodoShema          usssims    false    215   [       �          0    24664    User 
   TABLE DATA           G   COPY "TodoShema"."User" ("Id", "UserName", "PasswordHash") FROM stdin;
 	   TodoShema          usssims    false    216                     2606    24670    Task Task_pkey 
   CONSTRAINT     W   ALTER TABLE ONLY "TodoShema"."Task"
    ADD CONSTRAINT "Task_pkey" PRIMARY KEY ("Id");
 A   ALTER TABLE ONLY "TodoShema"."Task" DROP CONSTRAINT "Task_pkey";
    	   TodoShema            usssims    false    215                        2606    24672    User User_pkey 
   CONSTRAINT     W   ALTER TABLE ONLY "TodoShema"."User"
    ADD CONSTRAINT "User_pkey" PRIMARY KEY ("Id");
 A   ALTER TABLE ONLY "TodoShema"."User" DROP CONSTRAINT "User_pkey";
    	   TodoShema            usssims    false    216            "           2606    24674    User unique_username 
   CONSTRAINT     \   ALTER TABLE ONLY "TodoShema"."User"
    ADD CONSTRAINT unique_username UNIQUE ("UserName");
 E   ALTER TABLE ONLY "TodoShema"."User" DROP CONSTRAINT unique_username;
    	   TodoShema            usssims    false    216            #           2606    24675    Task FK_USER    FK CONSTRAINT     }   ALTER TABLE ONLY "TodoShema"."Task"
    ADD CONSTRAINT "FK_USER" FOREIGN KEY ("UserId") REFERENCES "TodoShema"."User"("Id");
 ?   ALTER TABLE ONLY "TodoShema"."Task" DROP CONSTRAINT "FK_USER";
    	   TodoShema          usssims    false    216    215    4640            �   �   x�e�A
�0�����@$�DEw��-6�M7B�-T�Z����X���~��&ZSm���fΦG�d��d�����`�^���m�����l(~6�q�8�m"AB�c���{�*T��YE���PW���<t��è�Dd
�~�J����D>9      �   �   x�5�KS�0 �ɯ���v<֠� �<,8^x|@��&� ��Ng�i/;;KP2�NGP[�؎�ߢQ+.�`]#�QGa������ug����+@�]�����q�}��g�gR����NOO���7�!SKJn�GI�=������Z�ŵ��̝X��UF1��H54�^$��Y��Nd�"���n��^������53����+�s>�㡀�)���������|aՊU-�>��0�ŎY�     