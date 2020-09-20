﻿/*
	AsterNET ARI Framework
	Automatically generated file @ 31/08/2020 12:42:41
*/
using System;
using System.Collections.Generic;
using SoftmakeAll.SDK.Asterisk.ARI.Models;
using SoftmakeAll.SDK.Asterisk.ARI;
using System.Threading.Tasks;

namespace SoftmakeAll.SDK.Asterisk.ARI.Actions
{
	
	public interface ISoundsActions
	{
		/// <summary>
		/// List all sounds.. 
		/// </summary>
		/// <param name="lang">Lookup sound for a specific language.</param>
		/// <param name="format">Lookup sound in a specific format.</param>
		List<Sound> List(string lang = null, string format = null);
		/// <summary>
		/// Get a sound's details.. 
		/// </summary>
		/// <param name="soundId">Sound's id</param>
		Sound Get(string soundId);

		/// <summary>
		/// List all sounds.. 
		/// </summary>
		/// <param name="lang">Lookup sound for a specific language.</param>
		/// <param name="format">Lookup sound in a specific format.</param>
		Task<List<Sound>> ListAsync(string lang = null, string format = null);
		/// <summary>
		/// Get a sound's details.. 
		/// </summary>
		/// <param name="soundId">Sound's id</param>
		Task<Sound> GetAsync(string soundId);
	}
}
